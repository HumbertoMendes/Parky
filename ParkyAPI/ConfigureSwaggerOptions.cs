﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace ParkyAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName, new OpenApiInfo()
                    {
                        Title = $"Parky API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
            }

            //var xmlCommentFile = Path.Combine(
            //    AppContext.BaseDirectory,
            //    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            //options.IncludeXmlComments(xmlCommentFile);
        }
    }
}
