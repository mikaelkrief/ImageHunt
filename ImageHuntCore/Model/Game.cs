using System;
using System.Collections.Generic;

namespace ImageHuntCore.Model
{
    public class Game : DbObject
    {
        public Game()
        {
            Nodes = new List<ImageHuntCore.Model.Node.Node>();
            Teams = new List<Team>();
        }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public string Name { get; set; }
        public Picture Picture { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public List<ImageHuntCore.Model.Node.Node> Nodes { get; set; }
        public List<Team> Teams { get; set; }
        public double? MapCenterLat { get; set; }
        public double? MapCenterLng { get; set; }
        public int? MapZoom { get; set; }
        public List<Passcode> Passcodes { get; set; }
        public int NbPlayerPenaltyThreshold { get; set; }
        public double NbPlayerPenaltyValue { get; set; }
        public string Code { get; set; }
    }
}
