﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RapidCMS.Common.Data;

namespace TestLibrary.Entities
{
    public class PersonEntity : IEntity
    {
#pragma warning disable IDE1006 // Naming Styles
        public int _Id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<PersonCountryEntity> Countries { get; set; }

        [NotMapped]
        string IEntity.Id { get => _Id.ToString(); set => _Id = int.Parse(value); }
    }
}
