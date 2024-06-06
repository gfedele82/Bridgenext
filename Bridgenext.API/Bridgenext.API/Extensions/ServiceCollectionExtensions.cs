﻿using Audit.Core;
using Audit.EntityFramework;
using Bridgenext.DataAccess;
using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.Repositories;
using Bridgenext.Engine;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Engine.Strategy;
using Bridgenext.Engine.Utils;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;
using Bridgenext.Models.Schema.Base.Audit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Bridgenext.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDocumentRepositoty, DocumentRepository>();

        }

        public static void RegisterEngines(this IServiceCollection services)
        {
            services.AddTransient<IUsersEngine, UsersEngine>();
            services.AddTransient<IDocumentEngine, DocumentEngine>();
            services.AddScoped<DocumentProcessDocument>();
            services.AddScoped<DocumentProcessImage>();
            services.AddScoped<DocumentProcessText>();
            services.AddScoped<DocumentProcessVideo>();
            services.AddScoped<DocumentTypeResolver>(ServicesProvider => fileTypes =>
            {
                return fileTypes switch
                {
                    FileTypes.Document => ServicesProvider.GetService<DocumentProcessDocument>(),
                    FileTypes.Video => ServicesProvider.GetService<DocumentProcessImage>(),
                    FileTypes.Text => ServicesProvider.GetService<DocumentProcessText>(),
                    FileTypes.Image => ServicesProvider.GetService<DocumentProcessVideo>(),
                    _ => throw new ArgumentException("File type not supported")
                }; ;
            });
        }


        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
            services.AddTransient<IValidator<string>, EmailRequestValidator>();
            services.AddTransient<IValidator<DeleteUserRequest>, DeleteUserRequestValidator>();
            services.AddTransient<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
            services.AddTransient<IValidator<CreateAddressRequest>, CreateAddressRequestValidator>();
            services.AddTransient<IValidator<UpdateAddressRequest>, UpdateAddressRequestValidator>();
        }

        public static void RegisterDatabaseContext(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var settings = configuration.GetSection(ConnectionStringsSettings.KEY).Get<ConnectionStringsSettings>();
            services.AddDbContext<UserSystemContext>(options => options.UseNpgsql(settings.BridgenextConnectionString), ServiceLifetime.Transient);

            Audit.Core.Configuration.Setup()
                .UseEntityFramework(e => e
                    .AuditTypeExplicitMapper(map => map
                        .Map<Users, Users_History>()
                        .Map<Addreesses, Addreesses_History>()
                        .Map<Documents, Documents_History>()
                    .AuditEntityAction<AuditableEntity>((ev, ent, auditEntity) =>
                    {
                        var entityFrameworkEvent = ev.GetEntityFrameworkEvent();
                        if (entityFrameworkEvent == null)
                        {
                            return;
                        }

                        auditEntity.AuditDate = DateTime.Now;
                        auditEntity.AuditAction = ent.Action;
                    })));
        }
    }
}