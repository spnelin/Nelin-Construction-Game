using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public partial class Game
    {
        private void Setup()
        {
            FillProjectPipeline();
            FillExecutiveDeck();
            FillOpportunityDeck();
        }

        private void FillProjectPipeline()
        {
            //30 for the newbies, in their eager wonder
            List<Project> t1proj = new List<Project>();
            t1proj.Add(new Project("Pedestrian Underpass", "A small tunnel under a road for pedestrian use.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Underpass", "A small tunnel under a road for pedestrian use.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Underpass", "A small tunnel under a road for pedestrian use.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Underpass", "A small tunnel under a road for pedestrian use.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Underpass", "A small tunnel under a road for pedestrian use.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Access Tunnel", "A small tunnel for city maintenance work.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Access Tunnel", "A small tunnel for city maintenance work.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Access Tunnel", "A small tunnel for city maintenance work.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Access Tunnel", "A small tunnel for city maintenance work.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Access Tunnel", "A small tunnel for city maintenance work.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Bike Trail Tunnel", "A small tunnel for a bike trail.", 2, 1, 0, 2, GenerateMaxBid(2, 1, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Bike Trail Tunnel", "A small tunnel for a bike trail.", 2, 1, 0, 2, GenerateMaxBid(2, 1, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Bike Trail Tunnel", "A small tunnel for a bike trail.", 2, 1, 0, 2, GenerateMaxBid(2, 1, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Bike Trail Tunnel", "A small tunnel for a bike trail.", 2, 1, 0, 2, GenerateMaxBid(2, 1, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Bike Trail Tunnel", "A small tunnel for a bike trail.", 2, 1, 0, 2, GenerateMaxBid(2, 1, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Bridge", "A small bridge for pedestrian use.", 0, 2, 0, 2, GenerateMaxBid(0, 2, 0), 1, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Bridge", "A small bridge for pedestrian use.", 0, 2, 0, 2, GenerateMaxBid(0, 2, 0), 1, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Bridge", "A small bridge for pedestrian use.", 0, 2, 0, 2, GenerateMaxBid(0, 2, 0), 1, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Bridge", "A small bridge for pedestrian use.", 0, 2, 0, 2, GenerateMaxBid(0, 2, 0), 1, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Bridge", "A small bridge for pedestrian use.", 0, 2, 0, 2, GenerateMaxBid(0, 2, 0), 1, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Overpass", "A pedestrian overpass across a road.", 0, 2, 1, 2, GenerateMaxBid(0, 2, 1), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Overpass", "A pedestrian overpass across a road.", 0, 2, 1, 2, GenerateMaxBid(0, 2, 1), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Overpass", "A pedestrian overpass across a road.", 0, 2, 1, 2, GenerateMaxBid(0, 2, 1), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Overpass", "A pedestrian overpass across a road.", 0, 2, 1, 2, GenerateMaxBid(0, 2, 1), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Pedestrian Overpass", "A pedestrian overpass across a road.", 0, 2, 1, 2, GenerateMaxBid(0, 2, 1), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Brook Bridge", "A small vehicle bridge crossing a brook.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Brook Bridge", "A small vehicle bridge crossing a brook.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Brook Bridge", "A small vehicle bridge crossing a brook.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Brook Bridge", "A small vehicle bridge crossing a brook.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Brook Bridge", "A small vehicle bridge crossing a brook.", 2, 2, 0, 2, GenerateMaxBid(2, 2, 0), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Park Shelter", "A shelter for parkgoers to rest in.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Park Shelter", "A shelter for parkgoers to rest in.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Park Shelter", "A shelter for parkgoers to rest in.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Park Shelter", "A shelter for parkgoers to rest in.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Park Shelter", "A shelter for parkgoers to rest in.", 2, 0, 0, 2, GenerateMaxBid(2, 0, 0), 1, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Post Office", "A small local post office.", 2, 0, 1, 2, GenerateMaxBid(2, 0, 1), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Post Office", "A small local post office.", 2, 0, 1, 2, GenerateMaxBid(2, 0, 1), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Post Office", "A small local post office.", 2, 0, 1, 2, GenerateMaxBid(2, 0, 1), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Post Office", "A small local post office.", 2, 0, 1, 2, GenerateMaxBid(2, 0, 1), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Post Office", "A small local post office.", 2, 0, 1, 2, GenerateMaxBid(2, 0, 1), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Community Center", "A small building for local activities.", 2, 0, 2, 2, GenerateMaxBid(2, 0, 2), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Community Center", "A small building for local activities.", 2, 0, 2, 2, GenerateMaxBid(2, 0, 2), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Community Center", "A small building for local activities.", 2, 0, 2, 2, GenerateMaxBid(2, 0, 2), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Community Center", "A small building for local activities.", 2, 0, 2, 2, GenerateMaxBid(2, 0, 2), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            t1proj.Add(new Project("Community Center", "A small building for local activities.", 2, 0, 2, 2, GenerateMaxBid(2, 0, 2), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>()));
            //15 for the players, in their habits set
            List<Project> t2proj = new List<Project>();
            ProjectRequirement req1;
            ProjectRequirement req2;
            req1 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier1, 2);
            req2 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier1, 1);
            t2proj.Add(new Project("Toll Bridge", "A bridge with a toll station.", 8, 8, 2, 5, GenerateMaxBid(8, 8, 2), 4, ProjectType.Bridge, ProjectTier.Tier2, new List<ProjectRequirement> { req1, req2 }));
            t2proj.Add(new Project("Highway Overpass", "A highway overpass, allowing traffic to pass.", 8, 10, 0, 5, GenerateMaxBid(8, 10, 0), 4, ProjectType.Bridge, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            t2proj.Add(new Project("Highway Overpass", "A highway overpass, allowing traffic to pass.", 8, 10, 0, 5, GenerateMaxBid(8, 10, 0), 4, ProjectType.Bridge, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier1, 1);
            t2proj.Add(new Project("River Footbridge", "A bridge allowing travelers to cross a river.", 8, 6, 0, 5, GenerateMaxBid(8, 6, 0), 3, ProjectType.Bridge, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            t2proj.Add(new Project("River Footbridge", "A bridge allowing travelers to cross a river.", 8, 6, 0, 5, GenerateMaxBid(8, 6, 0), 3, ProjectType.Bridge, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Tunnel, ProjectTier.Tier1, 1);
            t2proj.Add(new Project("Underground Parking", "A place for cars to go to sleep.", 8, 6, 0, 5, GenerateMaxBid(8, 6, 0), 3, ProjectType.Tunnel, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            t2proj.Add(new Project("Underground Parking", "A place for cars to go to sleep.", 8, 6, 0, 5, GenerateMaxBid(8, 6, 0), 3, ProjectType.Tunnel, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Tunnel, ProjectTier.Tier1, 2);
            t2proj.Add(new Project("Highway Tunnel", "A tunnel to save a highway a costly detour.", 12, 8, 0, 5, GenerateMaxBid(12, 8, 0), 4, ProjectType.Tunnel, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            t2proj.Add(new Project("Highway Tunnel", "A tunnel to save a highway a costly detour.", 12, 8, 0, 5, GenerateMaxBid(12, 8, 0), 4, ProjectType.Tunnel, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            req2 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier1, 1);
            t2proj.Add(new Project("Subway Station", "A small subway station serving commuters.", 10, 6, 2, 5, GenerateMaxBid(10, 6, 2), 4, ProjectType.Tunnel, ProjectTier.Tier2, new List<ProjectRequirement> { req1, req2 }));
            req1 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier1, 1);
            t2proj.Add(new Project("Branch Library", "A local libary, serving books to the masses.", 8, 4, 2, 5, GenerateMaxBid(8, 4, 2), 3, ProjectType.Building, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            t2proj.Add(new Project("Branch Library", "A local libary, serving books to the masses.", 8, 4, 2, 5, GenerateMaxBid(8, 4, 2), 3, ProjectType.Building, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier1, 2);
            t2proj.Add(new Project("Government Offices", "A tall office-building containing public service.", 8, 6, 4, 5, GenerateMaxBid(12, 8, 0), 4, ProjectType.Building, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            t2proj.Add(new Project("Government Offices", "A tall office-building containing public service.", 8, 6, 4, 5, GenerateMaxBid(12, 8, 0), 4, ProjectType.Building, ProjectTier.Tier2, new List<ProjectRequirement> { req1 }));
            req2 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier1, 1);
            t2proj.Add(new Project("Train Station", "A train hub, with attached shopping center.", 6, 6, 6, 5, GenerateMaxBid(6, 6, 6), 4, ProjectType.Building, ProjectTier.Tier2, new List<ProjectRequirement> { req1, req2 }));
            //10 for the veterans, intent in their plunder,
            List<Project> t3proj = new List<Project>();
            req1 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier2, 1);
            req2 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier2, 1);
            ProjectRequirement req3 = new ProjectRequirement(ProjectType.Tunnel, ProjectTier.Tier2, 1);
            t3proj.Add(new Project("International Airport", "A sprawling airport, its terminals connected by tunnels and bridges.", 20, 20, 20, 9, GenerateMaxBid(20, 20, 20), 7, ProjectType.Building, ProjectTier.Tier3, new List<ProjectRequirement> { req1, req2, req3 }));
            req1 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier2, 1);
            req2 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier2, 2);
            t3proj.Add(new Project("Commerce Towers", "Two towers for commercial enterprise, connected by a small bridge.", 10, 30, 24, 8, GenerateMaxBid(10, 30, 24), 8, ProjectType.Building, ProjectTier.Tier3, new List<ProjectRequirement> { req1, req2 }));
            req1 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier2, 2);
            t3proj.Add(new Project("Downtown Convention Center", "A massive building for hosting public and private functions.", 12, 20, 30, 9, GenerateMaxBid(12, 20, 30), 7, ProjectType.Building, ProjectTier.Tier3, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier2, 3);
            t3proj.Add(new Project("Metropolitan Public Hospital", "A great medical center to bring health to the people.", 16, 18, 26, 8, GenerateMaxBid(16, 18, 26), 8, ProjectType.Building, ProjectTier.Tier3, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Tunnel, ProjectTier.Tier2, 2);
            req2 = new ProjectRequirement(ProjectType.Building, ProjectTier.Tier2, 1);
            t3proj.Add(new Project("City Transit Hub", "The central hub of the city subway system.", 20, 30, 24, 11, GenerateMaxBid(20, 30, 24), 7, ProjectType.Tunnel, ProjectTier.Tier3, new List<ProjectRequirement> { req1, req2 }));
            t3proj.Add(new Project("Bay Tunnel Highway", "A tunnel guiding a highway across the bay.", 40, 30, 0, 10, GenerateMaxBid(40, 30, 0), 7, ProjectType.Tunnel, ProjectTier.Tier3, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Tunnel, ProjectTier.Tier2, 3);
            t3proj.Add(new Project("Metro Storm Basin", "A vast man-made cavern serving as a reservoir and storm drain for the city.", 44, 24, 0, 9, GenerateMaxBid(44, 24, 0), 8, ProjectType.Tunnel, ProjectTier.Tier3, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier2, 2);
            req2 = new ProjectRequirement(ProjectType.Tunnel, ProjectTier.Tier2, 1);
            t3proj.Add(new Project("Highway Bypass", "A highway split and guided around the city by a bridge and a tunnel.", 36, 34, 0, 10, GenerateMaxBid(36, 34, 0), 7, ProjectType.Bridge, ProjectTier.Tier3, new List<ProjectRequirement> { req1, req2 }));
            t3proj.Add(new Project("High-Speed Railway Bridge", "A bridge taking high-speed rail through the city.", 20, 50, 0, 10, GenerateMaxBid(20, 50, 0), 7, ProjectType.Bridge, ProjectTier.Tier3, new List<ProjectRequirement> { req1 }));
            req1 = new ProjectRequirement(ProjectType.Bridge, ProjectTier.Tier2, 3);
            t3proj.Add(new Project("Harbor Highway Bridge", "A vast bridge carrying a highway from one end of the harbor to the other.", 30, 40, 0, 9, GenerateMaxBid(30, 40, 0), 8, ProjectType.Bridge, ProjectTier.Tier3, new List<ProjectRequirement> { req1 }));
            //And one for keeping secret, exposed to no Get.
            //In the land of C#, where the Server lies.
            ProjectPipeline = new ProjectPipeline(t1proj, t2proj, t3proj);
        }

        private int GenerateMaxBid(int concrete, int steel, int glass)
        {
            int maxBid = 0;
            maxBid += concrete * Settings.ConcretePrice;
            maxBid += steel * Settings.SteelPrice;
            maxBid += glass * Settings.GlassPrice;
            maxBid += (concrete + steel + glass) * Settings.WorkerSalary;
            maxBid = maxBid * (10 + new Random().Next(9, 11)) / 10;
            maxBid += 10;
            return maxBid;
        }

        private int GeneratePrivateMaxBid(int concrete, int steel, int glass)
        {
            return (int)Math.Ceiling(GenerateMaxBid(concrete, steel, glass) * .75);
        }

        private void FillExecutiveDeck()
        {
            List<Executive> execs = new List<Executive>();

            execs.Add(new Executive("Working Stiff", "Doesn't do anything particularly well, but does it for not much money.", 12, ExecutiveAbility.None));
            execs.Add(new Executive("Working Stiff", "Doesn't do anything particularly well, but does it for not much money.", 12, ExecutiveAbility.None));
            execs.Add(new Executive("Working Stiff", "Doesn't do anything particularly well, but does it for not much money.", 12, ExecutiveAbility.None));
            execs.Add(new Executive("Working Stiff", "Doesn't do anything particularly well, but does it for not much money.", 12, ExecutiveAbility.None));

            execs.Add(new Executive("Concrete Hustler", "Hard-hearted when shopping for concrete. Reduces concrete prices by 20%.", 15, ExecutiveAbility.ReduceConcretePrice));
            execs.Add(new Executive("Concrete Hustler", "Hard-hearted when shopping for concrete. Reduces concrete prices by 20%.", 15, ExecutiveAbility.ReduceConcretePrice));
            execs.Add(new Executive("Concrete Hustler", "Hard-hearted when shopping for concrete. Reduces concrete prices by 20%.", 15, ExecutiveAbility.ReduceConcretePrice));

            execs.Add(new Executive("Lady Gray", "The undisputed mistress of concrete acquisition, with a heart as hard as her wares. Reduces concrete prices by 50%.", 22, ExecutiveAbility.ReduceConcretePricePlus));

            execs.Add(new Executive("Steel Seeker", "Can get you a steel of a deal. Reduces steel prices by 20%.", 15, ExecutiveAbility.ReduceSteelPrice));
            execs.Add(new Executive("Steel Seeker", "Can get you a steel of a deal. Reduces steel prices by 20%.", 15, ExecutiveAbility.ReduceSteelPrice));
            execs.Add(new Executive("Steel Seeker", "Can get you a steel of a deal. Reduces steel prices by 20%.", 15, ExecutiveAbility.ReduceSteelPrice));

            execs.Add(new Executive("Old Ironsides", "Steelworkers live in awe of him. Steel merchants live in fear of him. Reduces steel prices by 50%.", 22, ExecutiveAbility.ReduceSteelPricePlus));

            execs.Add(new Executive("Glass Hunter", "It's perfectly clear what this one can do. Reduces glass prices by 20%.", 15, ExecutiveAbility.ReduceGlassPrice));
            execs.Add(new Executive("Glass Hunter", "It's perfectly clear what this one can do. Reduces glass prices by 20%.", 15, ExecutiveAbility.ReduceGlassPrice));
            execs.Add(new Executive("Glass Hunter", "It's perfectly clear what this one can do. Reduces glass prices by 20%.", 15, ExecutiveAbility.ReduceGlassPrice));

            execs.Add(new Executive("The Glass Eye", "He sees through shoddy glass merchants easier than you can their product. Reduces glass prices by 50%.", 22, ExecutiveAbility.ReduceGlassPricePlus));

            execs.Add(new Executive("Accounting Artist", "With a little imagination, payroll taxes can go down while pay stays constant. Lowers salary expenses by 20%.", 20, ExecutiveAbility.ReduceSalaryExpenses));
            execs.Add(new Executive("Accounting Artist", "With a little imagination, payroll taxes can go down while pay stays constant. Lowers salary expenses by 20%.", 20, ExecutiveAbility.ReduceSalaryExpenses));

            execs.Add(new Executive("Vigorous Opportunist", "Never stops hunting for the next big chance. When searching for opportunities, reveals an extra one.", 20, ExecutiveAbility.BetterOpportunityFinding));
            execs.Add(new Executive("Vigorous Opportunist", "Never stops hunting for the next big chance. When searching for opportunities, reveals an extra one.", 20, ExecutiveAbility.BetterOpportunityFinding));

            execs.Add(new Executive("Beaming Socialite", "Has friends in high places, and every other place as well. When searching for executives, reveals an extra one.", 20, ExecutiveAbility.BetterExecutiveHiring));
            execs.Add(new Executive("Beaming Socialite", "Has friends in high places, and every other place as well. When searching for executives, reveals an extra one.", 20, ExecutiveAbility.BetterExecutiveHiring));

            execs.Add(new Executive("Towering Architect", "Shares his stature with the buildings he envisions. Requires half the usual workers on building-type projects.", 20, ExecutiveAbility.BetterBuildingBuilding));
            execs.Add(new Executive("Towering Architect", "Shares her stature with the buildings she envisions. Requires half the usual workers on building-type projects.", 20, ExecutiveAbility.BetterBuildingBuilding));

            execs.Add(new Executive("Boring Expert", "Rather dull, but does know his way around a drill. Requires half the usual workers on tunnel-type projects.", 20, ExecutiveAbility.BetterTunnelBuilding));
            execs.Add(new Executive("Boring Expert", "Rather dull, but does know her way around a drill. Requires half the usual workers on tunnel-type projects.", 20, ExecutiveAbility.BetterTunnelBuilding));

            execs.Add(new Executive("Lofty Engineer", "His manners, and his creations, have an airy grace to them. Requires half the usual workers on bridge-type projects.", 20, ExecutiveAbility.BetterBridgeBuilding));
            execs.Add(new Executive("Lofty Engineer", "Her manners, and her creations, have an airy grace to them. Requires half the usual workers on bridge-type projects.", 20, ExecutiveAbility.BetterBridgeBuilding));

            ExecutiveDeck = new Deck<Executive>(execs);
        }

        private void FillOpportunityDeck()
        {
            List<Opportunity> opportunities = new List<Opportunity>();

            opportunities.Add(new MaterialDealOpportunity("Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 10));

            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 30));
            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 30));
            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Concrete", "A solid offering - 20% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscount, 30));

            opportunities.Add(new MaterialDealOpportunity("Heavily Discounted Concrete", "Hard to pass up - 50% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscountPlus, 10));
            opportunities.Add(new MaterialDealOpportunity("Heavily Discounted Concrete", "Hard to pass up - 50% off the usual price.", MaterialType.Concrete, Settings.ConcretePriceDiscountPlus, 10));

            opportunities.Add(new MaterialDealOpportunity("Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 10));

            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 30));
            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 30));
            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Steel", "This is a steal - 20% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscount, 30));

            opportunities.Add(new MaterialDealOpportunity("Heavily Discounted Steel", "This will leave you beaming - 50% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscountPlus, 10));
            opportunities.Add(new MaterialDealOpportunity("Heavily Discounted Steel", "This will leave you beaming - 50% off the usual price.", MaterialType.Steel, Settings.SteelPriceDiscountPlus, 10));

            opportunities.Add(new MaterialDealOpportunity("Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 10));
            opportunities.Add(new MaterialDealOpportunity("Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 10));

            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 30));
            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 30));
            opportunities.Add(new MaterialDealOpportunity("Bulk Discounted Glass", "Clearly a good deal - 20% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscount, 30));

            opportunities.Add(new MaterialDealOpportunity("Heavily Discounted Glass", "A brilliant bargain - 50% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscountPlus, 10));
            opportunities.Add(new MaterialDealOpportunity("Heavily Discounted Glass", "A brilliant bargain - 50% off the usual price.", MaterialType.Glass, Settings.GlassPriceDiscountPlus, 10));

            opportunities.Add(new MoneyWindfallOpportunity("Private Investors", "Some affluent urbanites are interested in your stock. Their support will prove useful indeed.", 75));
            opportunities.Add(new MoneyWindfallOpportunity("Private Investors", "Some affluent urbanites are interested in your stock. Their support will prove useful indeed.", 75));
            opportunities.Add(new MoneyWindfallOpportunity("Private Investors", "Some affluent urbanites are interested in your stock. Their support will prove useful indeed.", 75));

            opportunities.Add(new MoneyWindfallOpportunity("Venture Capital", "\"We can supply funding for your enterprise,\" says a gaunt man in a black suit. \"Just sign here.\"", 150));
            opportunities.Add(new MoneyWindfallOpportunity("Venture Capital", "\"We can supply funding for your enterprise,\" says a gaunt woman in a black suit. \"Just sign here.\"", 150));

            opportunities.Add(new MoneyWindfallOpportunity("* TO * THE * MOON *", "It's unclear exactly why, but many small investors on the internet are buying shares in your company. Best not to think too much about it.", 250));

            opportunities.Add(new ExecutiveHireOpportunity("College Job Fair", "Not everyone is promising enough to hire straight out of college, but you can get them for less pay.", 2, .6));
            opportunities.Add(new ExecutiveHireOpportunity("College Job Fair", "Not everyone is promising enough to hire straight out of college, but you can get them for less pay.", 2, .6));

            opportunities.Add(new ExecutiveHireOpportunity("Recruiting Agency", "They can scare up a lot of prospects, but they take their cut.", 6, 1.1));
            opportunities.Add(new ExecutiveHireOpportunity("Recruiting Agency", "They can scare up a lot of prospects, but they take their cut.", 6, 1.1));

            opportunities.Add(new ExecutiveHireOpportunity("Company Referrals", "A friend of a friend, or so. They come recommended, at acceptable pay.", 3, .8));
            opportunities.Add(new ExecutiveHireOpportunity("Company Referrals", "A friend of a friend, or so. They come recommended, at acceptable pay.", 3, .8));

            Project bankOffice = new Project("Bank Branch Office", "A small commercial building for a bank.", 4, 1, 1, 3, GeneratePrivateMaxBid(4, 1, 1), 2, ProjectType.Building, ProjectTier.Tier1, new List<ProjectRequirement>());
            opportunities.Add(new PrivateProjectOpportunity("Bank Branch Office", "It's small, it's square, and they'd love you to help build it.", bankOffice));
            opportunities.Add(new PrivateProjectOpportunity("Bank Branch Office", "It's small, it's square, and they'd love you to help build it.", bankOffice));

            Project businessWalkway = new Project("Corporate Walkway", "An enclosed walkway connecting two private buildings.", 0, 3, 3, 3, GeneratePrivateMaxBid(0, 3, 3), 2, ProjectType.Bridge, ProjectTier.Tier1, new List<ProjectRequirement>());
            opportunities.Add(new PrivateProjectOpportunity("Corporate Walkway", "They're trying to build bridges between Sales and Development - literally, in this case. You can help.", businessWalkway));
            opportunities.Add(new PrivateProjectOpportunity("Corporate Walkway", "They're trying to build bridges between Sales and Development - literally, in this case. You can help.", businessWalkway));

            Project utilityTunnel = new Project("Industrial Utility Tunnel", "A tunnel connecting two buildings in a factory.", 3, 3, 0, 3, GeneratePrivateMaxBid(3, 3, 0), 2, ProjectType.Tunnel, ProjectTier.Tier1, new List<ProjectRequirement>());
            opportunities.Add(new PrivateProjectOpportunity("Bank Branch Office", "Most people would find this boring, but it's important enough to them that they'll pay you for it.", utilityTunnel));
            opportunities.Add(new PrivateProjectOpportunity("Bank Branch Office", "Most people would find this boring, but it's important enough to them that they'll pay you for it.", utilityTunnel));

            OpportunityDeck = new Deck<Opportunity>(opportunities);
        }
    }
}
