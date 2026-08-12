using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.Specializations;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using NUnit.Framework;
using Name = dddnet8.Domain.OperationTypes.Names.Name;

namespace test.Domain.RequiredStaffs
{
    public class RequiredStaffTest
    {

        OperationType op1;
        Guid guid;

        [SetUp]
        public void Setup()
        {
            op1 = new OperationType(Guid.NewGuid(), new Name("Nome Teste"), Status.Active, new EstimatedDuration(new TimeSpan(1,30,0), new System.TimeSpan(1, 30, 0), new System.TimeSpan(1, 30, 0)), OperationTypeCode.Create("OT0001"));
            guid = Guid.NewGuid();
        }

        [Test]
        public void ThrowsIfQuantityIsZero()
        {
            Specialization specialization = new Specialization(dddnet8.Domain.Patients.V.O.Name.Create("Pediatrics"), Description.Create("Hello").Value, SpecializationCode.Create("TESte") );
            Exception e = Assert.Throws<ArgumentException>( () => {
                RequiredStaffQuantity quantity = new RequiredStaffQuantity(0);
                RequiredStaff requiredStaff = new RequiredStaff(guid, specialization, quantity, op1);
            });
        }

        [Test]
        public void ThrowsIfOperationTypeIsNull()
        {
            Specialization specialization = new Specialization(dddnet8.Domain.Patients.V.O.Name.Create("Pediatrics"), Description.Create("Hello").Value, SpecializationCode.Create("TESte") );
            RequiredStaffQuantity quantity = new RequiredStaffQuantity(1);

            Exception e = Assert.Throws<ArgumentNullException>(() =>
            {
                RequiredStaff requiredStaff = new RequiredStaff(guid, specialization, quantity, null);
            });
        }
    }
}