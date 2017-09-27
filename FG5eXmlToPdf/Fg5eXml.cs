﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using FG5eXmlToPdf;
using FG5eXmlToPdf.Models;
using FG5eXmlToPDF.Models;
// ReSharper disable All

namespace FG5eXmlToPDF
{
    public static class Fg5eXml
    {

        public static Character5e LoadCharacter(string fileString)
        {
            var xml = XDocument.Load(fileString);
            _charElement = xml?.Root?.Element("character");


            var character = new Character5e();
            foreach (var prop in character.Properties.ToList())
            {
                character.Properties[prop.Key] = GetCharValue(prop.Key);
            }


            var abilityList = xml.Root.Element("character")?.Element("abilities").Elements().ToList();
            foreach (var attrib in abilityList)
            {
                character.Abilities.Add(new Ability()
                {
                    Name = attrib.Name.ToString(),
                    Score = int.Parse(attrib.Element("score").Value),
                    Bonus = int.Parse(attrib.Element("bonus").Value),
                    Save = int.Parse(attrib.Element("save").Value),
                    Savemodifier = int.Parse(attrib.Element("savemodifier").Value),
                    Saveprof = Helper.StringIntToBool(attrib.Element("saveprof").Value)
                });
            }

            var skillList = xml.Root.Element("character")?.Element("skilllist").Elements().ToList();
            foreach (var skill in skillList)
            {
                character.Skills.Add(new Skill()
                {
                    Name = skill.Element("name").Value,
                    Misc = int.Parse(skill.Element("misc").Value),
                    Prof = Helper.StringIntToBool(skill.Element("prof").Value),
                    Stat = skill.Element("stat").Value,
                    Total = int.Parse(skill.Element("total").Value)
                });
            }

            return character;
        }


        private static XElement _charElement;

        private static string GetCharValue(string name)
        {
            //everything is lowercase as far as I can tell in the xml
            return _charElement.Element(name.ToLower())?.Value ?? string.Empty;
        }
    }
}
