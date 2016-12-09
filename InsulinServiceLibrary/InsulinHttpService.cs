using InsulinServiceLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;

namespace InsulinServiceLibrary
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class InsulinHttpService : IInsulinHttpService
    {
        private class Locale
        {
            public string localeString;
            public bool hasLangageCode;
            public string countryCode;
        }

        #region static data

        private static string appDataPath;
        private static string AppDataPath
        {
            get
            {
                if (String.IsNullOrEmpty(appDataPath))
                {
                    appDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
                    if (!File.Exists(Path.Combine(appDataPath, "Insulin.json")))
                    {
                        Console.WriteLine("'App_Data' directory not found.");
                    }
                }
                return appDataPath;
            }
        }

        private static ApiReference apiDocumentation;
        private static ApiReference ApiDocumentation
        {
            get
            {
                if (apiDocumentation == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "InsulinApi.json"));
                    apiDocumentation = JsonConvert.DeserializeObject<ApiReference>(json);
                }
                return apiDocumentation;
            }
        }

        private static IList<CountryCode> countryCodes;
        private static IList<CountryCode> CountryCodes
        {
            get
            {
                if (countryCodes == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "InsulinCountries.json"));
                    countryCodes = JsonConvert.DeserializeObject<List<CountryCode>>(json);
                }
                return countryCodes;
            }
        }

        private static IList<Insulin> insulinList;
        private static IList<Insulin> InsulinList
        {
            get
            {
                if (insulinList == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "Insulin.json"));
                    insulinList = JsonConvert.DeserializeObject<List<Insulin>>(json);
                }
                return insulinList;
            }
        }

        private static IList<InsulinCategoryTranslation> insulinCategoryTranslations;
        private static IList<InsulinCategoryTranslation> InsulinCategoryTranslations
        {
            get
            {
                if (insulinCategoryTranslations == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "InsulinCategoryTranslations.json"));
                    insulinCategoryTranslations = JsonConvert.DeserializeObject<List<InsulinCategoryTranslation>>(json);
                }
                return insulinCategoryTranslations;
            }
        }

        private static IList<InsulinCategory> insulinCategories;
        private static IList<InsulinCategory> InsulnCategories
        {
            get
            {
                if (insulinCategories == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "InsulinCategories.json"));
                    insulinCategories = JsonConvert.DeserializeObject<List<InsulinCategory>>(json);
                }
                return insulinCategories;
            }
        }

        private static IList<InsulinCountryMap> insulinCountryMap;
        private static IList<InsulinCountryMap> InsulinCountryMap
        {
            get
            {
                if (insulinCountryMap == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "InsulinCountryMap.json"));
                    insulinCountryMap = JsonConvert.DeserializeObject<List<InsulinCountryMap>>(json);
                }
                return insulinCountryMap;
            }
        }

        private static IList<InsulinNameTranslation> insulinNameTranslations;
        private static IList<InsulinNameTranslation> InsulinNameTranslations
        {
            get
            {
                if (insulinNameTranslations == null)
                {
                    string json = File.ReadAllText(Path.Combine(AppDataPath, "InsulinNameTranslations.json"));
                    insulinNameTranslations = JsonConvert.DeserializeObject<List<InsulinNameTranslation>>(json);
                }
                return insulinNameTranslations;
            }
        }

        #endregion


        public ApiReference GetApiDocumentation()
        {
            return ApiDocumentation;
        }

        public IEnumerable<InsulinCategory> GetInsulinCategories()
        {
            return InsulnCategories;
        }

        public IEnumerable<InsulinCategory> GetInsulinCategoriesByCountry(string countrycode)
        {
            var insulinCategoryViewList = new List<InsulinCategory>();

            var locale = GetLocale(countrycode);
            if (locale == null)
            {
                return insulinCategoryViewList;
            }

            // get the categories
            foreach (var insulinCategory in InsulnCategories)
            {
                string categoryTranslation = GetInsulinCategoryTranslation(insulinCategory.TypeKey, locale);
                if (string.IsNullOrEmpty(categoryTranslation))
                {
                    categoryTranslation = insulinCategory.Name;
                }

                insulinCategoryViewList.Add(
                    new InsulinCategory
                    {
                        Name = categoryTranslation,
                        CategoryTypeId = insulinCategory.CategoryTypeId,
                        TypeKey = insulinCategory.TypeKey,
                        Slot = insulinCategory.Slot,
                        SlotTypeKey = insulinCategory.SlotTypeKey
                    }
                );
            }
            return insulinCategoryViewList;
        }

        public IEnumerable<CountryCode> GetInsulinCountries()
        {
            return CountryCodes;
        }

        public IEnumerable<Insulin> GetInsulinList()
        {
            return InsulinList;
        }

        public IEnumerable<Insulin> GetInsulinByCountry(string countrycode)
        {
            IList<Insulin> insulinReturnList = new List<Insulin>();
            var locale = GetLocale(countrycode);
            if (locale == null)
            {
                return insulinReturnList;
            }

           
            var insulinCountries = InsulinCountryMap.Where(ic => ic.CountryCode == locale.countryCode).ToList();

            if (insulinCountries == null || insulinCountries.Count == 0)
            {
                // if the country code is valid, return ROW
                if (getValidCountryCodes().Contains(locale.countryCode))
                {
                    // nothing found for the given country code, for now return ROW
                    insulinCountries = InsulinCountryMap.Where(ic => ic.CountryCode.Equals("ROW", StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    return insulinReturnList;
                }
            }

            foreach (var insulinCountry in insulinCountries)
            {
                var insulin = InsulinList.SingleOrDefault(ins => ins.GUID.Equals(insulinCountry.InsulinGUID));

                string insulinNameTranslation = GetInsulinNameTranslation(insulin.GUID, locale);
                if (string.IsNullOrEmpty(insulinNameTranslation))
                {
                    insulinNameTranslation = insulin.Name;
                }

                string categoryTranslation = GetInsulinCategoryTranslation(insulin.CategoryTypeKey, locale);
                if (string.IsNullOrEmpty(categoryTranslation))
                {
                    categoryTranslation = insulin.CategoryName;
                }

                insulinReturnList.Add(
                    new Insulin
                    {
                        GUID = insulin.GUID,
                        Name = insulinNameTranslation,
                        CategoryName = categoryTranslation,
                        CategoryTypeKey = insulin.CategoryTypeKey
                    }
                 );
            }
            return insulinReturnList;
        }

        public Insulin GetInsulin(string id)
        {
            Guid insulinGuid = Guid.Parse(id);
            var ret = InsulinList.FirstOrDefault(ins => ins.GUID == insulinGuid);
            return ret;
        }

        #region Private Methods

        private Locale GetLocale(string localeString)
        {
            string upperLocale = localeString.ToUpperInvariant();
            Locale locale = new Locale { localeString = upperLocale };

            if (upperLocale.Equals("ROW"))
            {
                locale.hasLangageCode = false;
                locale.countryCode = "US";
            }
            else if (upperLocale.Length == 2)
            {
                locale.hasLangageCode = false;
                locale.countryCode = upperLocale;
            }
            else if (upperLocale.Length == 5)
            {
                string[] parts = upperLocale.Split('-');
                if (parts.Length == 2)
                {
                    locale.hasLangageCode = true;
                    locale.countryCode = parts[1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            return locale;
        }

        private string GetInsulinNameTranslation(Guid guid, Locale locale)
        {
            var translations = InsulinNameTranslations.Where(tr => tr.GUID == guid);
            InsulinNameTranslation translation = null;

            if (locale.hasLangageCode)
            {
                // try the full locale string
                translation = translations.SingleOrDefault(tr => tr.Code.Equals(locale.localeString, StringComparison.OrdinalIgnoreCase));
                if (translation != null)
                {
                    return translation.Translation;
                }
            }

            // try just the country code
            translation = translations.SingleOrDefault(tr => tr.Code.Equals(locale.countryCode, StringComparison.OrdinalIgnoreCase));
            if (translation != null)
            {
                return translation.Translation;
            }
            return string.Empty;
        }

        private string GetInsulinCategoryTranslation(string typeKey, Locale locale)
        {
            InsulinCategoryTranslation translation = null;
            var translations = InsulinCategoryTranslations.Where(trs => trs.TypeKey == typeKey);

            if (locale.hasLangageCode)
            {
                // try the full locale string
                translation = translations.SingleOrDefault(tr => tr.Code.Equals(locale.localeString, StringComparison.InvariantCultureIgnoreCase));
                if (translation != null)
                {
                    return translation.Translation;
                }
            }

            if (translation == null)
            {
                translation = translations.SingleOrDefault(tr => tr.Code.Equals(locale.countryCode, StringComparison.InvariantCultureIgnoreCase));
                if (translation != null)
                {
                    return translation.Translation;
                }
            }
            return string.Empty;
        }

        private static IList<string> validCountryCodes = null;
        private IList<string> getValidCountryCodes()
        {
            if (validCountryCodes == null)
            {
                validCountryCodes = new List<string>();

                var client = new HttpClient();
                var task = client.GetAsync("http://country.io/names.json")
                    .ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        string resultString = jsonString.Result;
                        var index = 0;
                        index = resultString.IndexOf(':', index + 1);

                        while (index != -1)
                        {
                            string countryCode = resultString.Substring(index - 3, 2);
                            validCountryCodes.Add(countryCode);
                            index = resultString.IndexOf(':', index + 1);
                        }
                    });

                task.Wait();
            }

            return validCountryCodes;
        }
        #endregion
    }
}
