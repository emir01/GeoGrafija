using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdministrationPanel.Model;
using Common.Static_Dictionary;
using Model;
using Model.Repositories;

namespace AdministrationPanel
{
    public partial class Form1 : Form
    {
        public GeoGrafijaEntities Context;
        public LocationsRepository LocationsRepository;
        public UserRepository UserRepository;
        
        private List<string> _locationStrings = new List<string>();
        private  List<Location>  _locations= new List<Location>();

        private const string NOT_DEFINED = "VALUE NOT DEFINED";
        public Form1()
        {
            InitializeComponent();
            Context = new GeoGrafijaEntities();
            LocationsRepository = new LocationsRepository(Context);
            UserRepository = new UserRepository(Context);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load type data
            var allLocationTypes = LocationsRepository.getAllLocationTypes().ToList();
            comboLocationType.DataSource = allLocationTypes;
            comboLocationType.ValueMember = "ID";
            comboLocationType.DisplayMember = "TypeName";

            // Set user data
            var allUsers = UserRepository.GetAllUsers().Where(x=>x.UserHasRole(RoleNames.TEACHER)).ToList();
            comboUsers.DataSource = allUsers;
            comboUsers.ValueMember = "UserID";
            comboUsers.DisplayMember = "UserName";
        }

        private void btnProcessInput_Click(object sender, EventArgs e)
        {
            _locationStrings.Clear();
            _locations.Clear();
            tbox_Output.Clear();

            if (fileDialog.FileName != null)
            {
                StreamReader streamReader;
                try
                {
                    streamReader = new StreamReader(fileDialog.OpenFile());
                }
                catch (Exception ex)
                {
                    return;
                }

                string line;
                
                while ((line = streamReader.ReadLine()) != null)
                {
                    var outputString = "";
                    foreach (char c in line)
                    {
                        if (Char.IsDigit(c) || Char.IsLetter(c) || char.IsWhiteSpace(c) || c.Equals(',')  || c.Equals('|'))
                        {
                            if(c == ' ' && outputString[outputString.Length-1] == ' ')
                                return;
                            
                            outputString += c;
                        }
                        else
                        {
                        }
                    }

                    _locationStrings.Add(outputString);
                    var location = GetLocationFromString(outputString);
                    if (location != null)
                    {
                        _locations.Add(location);
                    }
                }

                tbox_Output.Lines = _locationStrings.ToArray();
                gridLocations.DataSource = _locations;
                gridLocations.Refresh();
            }
            else
            {
                return;
            }
        }

        private Location GetLocationFromString(string outputString)
        {
            var splitPart = outputString.Split('|').ToList();
            if (splitPart.Count > 2) { 
                return null;
            }

            var nameString = splitPart[0];
            var coordinatesString = splitPart[1];

            var locationName = GetLocationName(nameString);
            var coordinates = GetLocationCoordinates(coordinatesString);

            if (locationName == null || coordinates == null)
            {
                return null;
            }

            var location = GetLocationFromValues(locationName, coordinates);

            return location;
        }

        private Location GetLocationFromValues(string locationName, Coordinates coordinates)
        {
            var loc = new Location();

            loc.Name = locationName;

            loc.Lat = coordinates.Lat;
            loc.Lng = coordinates.Lng;

            // get the type
            loc.LocationType = (int)comboLocationType.SelectedValue;
            
            int parentId = 0;
            int.TryParse(tboxLocationsParentId.Text, out parentId);
            loc.ParentId = parentId;

            return loc;
        }

        private Coordinates GetLocationCoordinates(string coordinatesString)
        {
            var coord = new Coordinates();

            coordinatesString = coordinatesString.ToLower();
            var split = coordinatesString.Split(',');
            
            var lng = split[0].Trim();
            var lat = split[1].Trim();

            var calculateLng = GetCoord(lng);
            var calculateLat = GetCoord(lat);

            coord.Lat = calculateLat;
            coord.Lng = calculateLng;
            
            return coord;
        }

        private double GetCoord(string coordString)
        {
            var indicator = Char.ToLower(coordString[coordString.Length - 1]);
            var processed = coordString.Substring(0, coordString.Length - 1).Trim();

            var steps = processed.Split(' ').ToList();

            double baseValue = 0.0;
            double minuteValue = 0.0;
            double secondValue = 0.0;

            if (steps.Count >= 1) 
            { 
                Double.TryParse(steps[0], out baseValue);
            }

            if (steps.Count >= 2)
            {
                Double.TryParse(steps[1], out minuteValue);
            }

            if (steps.Count == 3)
            {
                Double.TryParse(steps[2], out secondValue);
            }

            minuteValue = (double)(minuteValue/60);
            secondValue = (double)(secondValue / 3600);

            double  finalValue = 0.0; 
            finalValue = baseValue + minuteValue + secondValue;

            // find the negativity.
            if (indicator.Equals('s') || indicator.Equals('w'))
            {
                finalValue = finalValue*(-1);
            }
            return finalValue;
        }

        private string GetLocationName(string nameString)
        {
            var split = nameString.Split(',').ToList();

            if (split.Count > 1)
            {
                return null;
            }
            else
            {
                return split[0].Trim();
            }
        }

        private void btnCreateData_Click(object sender, EventArgs e)
        {
            int parentId = 0;
            Int32.TryParse(tboxLocationsParentId.Text,out parentId);

            var parentLocation = Context.Locations.Where(x => x.ID == parentId).FirstOrDefault();

            var children = parentLocation.Children.ToList();
            parentLocation.Children.Clear();

            foreach (var location in children)
            {
                Context.Locations.DeleteObject(location);    
            }

            var creatorId = (int)(comboUsers.SelectedValue);
            foreach (var location in _locations)
            {
                location.CreatedBy = creatorId;
                parentLocation.Children.Add(location);
            }

            Context.SaveChanges();
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            fileDialog.ShowDialog();
        }

        private void fileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var fileName = fileDialog.FileName;
            tboxFileBox.Text = fileName;
        }

        private void gridLocations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cell = gridLocations[e.RowIndex, e.ColumnIndex];

            if (cell.Value == null || cell.Value == "")
            {
                cell.Value = NOT_DEFINED;
            }
        }
    }
}
