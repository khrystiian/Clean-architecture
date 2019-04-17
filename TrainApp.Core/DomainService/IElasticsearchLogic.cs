using Elasticsearch.Net;
using Nest;

namespace TrainApp.Core.DomainService
{
    public interface IElasticsearchLogic
    {
        #region LowlevelElasticsearch
        bool IndexToNestElasticsearch(IElasticClient client);
        string IndexSearch(string input);
        string IndexSearch2();
        #endregion


        #region NEST
        bool IndexToElasticsearch(IElasticLowLevelClient client);
        void NestIndexSearch();
        void NestIndexSearch2();
        #endregion
    }
}
