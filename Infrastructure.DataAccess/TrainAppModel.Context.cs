﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Infrastructure.DataAccess
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class TrainAppEntities : DbContext
{
    public TrainAppEntities()
        : base("name=TrainAppEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<PassengersAge> PassengersAges { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteSeat> RouteSeats { get; set; }

    public virtual DbSet<TransitDetail> TransitDetails { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

}

}

