﻿using Microsoft.EntityFrameworkCore;
using NCBack.Models;

namespace NCBack.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
    public virtual DbSet<User> Users { get; set; }
}

