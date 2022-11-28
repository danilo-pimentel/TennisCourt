using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.Data.Context.Configurations;

namespace TennisCourt.Infra.Data.Context
{
    internal static class TennisCourtModelBuilder
    {

        public static ModelBuilder BuildModels(this ModelBuilder builder)
        {
            // reservation entity
            builder.ApplyConfiguration(new ReservationConfiguration());

            return builder;
        }
    }
}
