﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoDeepDive.Data;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using DtoDeepDive.Data;

namespace DtoDeepDive.UnitTests {
    public class Parts_catalog_dto_assembler : IDisposable {
        private readonly ITestOutputHelper _output;
        private readonly IPartsCatalogDbContext _db;
        public Parts_catalog_dto_assembler(ITestOutputHelper output) {
            _output = output;
            _db = new FakePartsCatalogDbContext();
            for (int i = 0; i < 10; i++) {
                Part part = new Part() {
                    PartNumber = "TEST-PART-NUMBER|" + i
                };
                for (int j = 0; j < 5; j++) {
                    var component = new Component() {
                        ComponentNumber = "TEST-COMPONENT-NUMBER|" + j
                    };
                    part.Components.Add(component);
                }
                for (int j = 0; j < 5; j++) {
                    var laborSequence = new LaborSequence() {
                        LaborSequenceNumber = "LABOR-SEQUENCE-NUMBER|" + j
                    };
                    part.LaborSequences.Add(laborSequence);
                }
                _db.Parts.Add(part);
            }
            _db.SaveChanges();
        }
        [Fact]
        public void Get_part_dto_from_assembler() {
            var repository = new PartRepository(_db);
            var assembler = new PartAssembler();
            var partCatalogService = new PartCatalogService(repository, assembler);

            var partDto = partCatalogService.GetPart("TEST-PART-NUMBER|1");
            partDto.Should().BeOfType<PartDTO>();
        }
        public void Dispose() {
            _db.Dispose();
        }
    }
}