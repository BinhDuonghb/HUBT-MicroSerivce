using MongoDB.Driver;
using HUBT_Social_MongoDb_Service.ASP_Extentions;
using HUBT_Social_Core.Settings;
using Identity_API.Src.Models;

namespace Identity_API.Configurations;

public static class MongoDbConfiguration
{
    public static IServiceCollection AddMongoCollections(this IServiceCollection services,
        IConfiguration configuration)
    {
        DatabaseSetting? connectionstring = configuration.GetSection("TempRegisterDB").Get<DatabaseSetting>();

        if (connectionstring != null)
        {
            services.RegisterMongoCollections(connectionstring, typeof(UserToken));
            return services;
        }
        throw new Exception("Unable to genarate Mongodb");

    }

}