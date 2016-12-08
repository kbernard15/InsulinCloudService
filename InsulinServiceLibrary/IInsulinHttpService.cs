using InsulinServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InsulinServiceLibrary
{
    [ServiceContract]
    public interface IInsulinHttpService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin", ResponseFormat = WebMessageFormat.Json)]
        ApiReference GetApiDocumentation();

        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin/categories", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<InsulinCategory> GetInsulinCategories();

        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin/categories/{countrycode}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<InsulinCategory> GetInsulinCategoriesByCountry(string countrycode);

        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin/countries", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<CountryCode> GetInsulinCountries();

        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin/list", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<Insulin> GetInsulinList();

        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin/{countrycode}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<Insulin> GetInsulinByCountry(string countrycode);

        [OperationContract]
        [WebGet(UriTemplate = "/api/insulin/insulin/{id}", ResponseFormat = WebMessageFormat.Json)]
        Insulin GetInsulin(string id);
    }
    // Data Contract classes are in the Models folder.
}
