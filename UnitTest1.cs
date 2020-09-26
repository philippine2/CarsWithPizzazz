using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SUT = CarsWithPizzazz;
using CarsWithPizzazz;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
namespace UnitTestCarsWithPizza
{
    [TestClass]
    public class AutoControl_Should

    {
        public List<SUT.Auto> AutoListTest()
        {

            //create list of cars to return

            var CarsList = new List<SUT.Auto>();
            CarsList.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "01xxxxxxxxxxxxxxx",
                Make = "Cadillac",
                Year = "2008",
                Model = "CTS-V",
                LocationOnLot = "A5"
            });

            CarsList.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "02xxxxxxxxxxxxxxx",
                Make = "Doudge",
                Year = "1964",
                Model = "Dart",
                LocationOnLot = "F3"
            });
            CarsList.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "03xxxxxxxxxxxxxxx",
                Make = "Cadillac",
                Year = "1963",
                Model = "Fleetwood",
                LocationOnLot = "A23"
            });
            CarsList.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "04xxxxxxxxxxxxxxx",
                Make = "Hummer",
                Year = "1995",
                Model = "H1(Gas)",
                LocationOnLot = "C7"
            });
            CarsList.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "05xxxxxxxxxxxxxxx",
                Make = "Triumph",
                Year = "1958",
                Model = "TR3",
                LocationOnLot = "A1"
            });
            CarsList.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "06xxxxxxxxxxxxxxx",
                Make = "Triumph",
                Year = "1968",
                Model = "TR5",
                LocationOnLot = "A2"
            });

            return CarsList;
        }

        [TestMethod]
        public void ReturnInstance_WhenRequestedCarIsFound()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myMockDBA = new Mock<SUT.IAutoDBAccess>();
            myMockDBA.Setup(x => x.LoadLot()).Returns(AutoListTest());
            var myFindCar = new SUT.AutoControl(myMockDBA.Object);

            //Act
            var result = myFindCar.FindCar("06xxxxxxxxxxxxxxx");

            //Assert

            Assert.AreEqual("06xxxxxxxxxxxxxxx", result.VehicleIdentificationNumber);

        }

        [TestMethod]
        public void ThrowVinNOtFoundExcepction_WhenRequestedCarIsNotFound()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
          
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());

           //Act
            var findCar = new SUT.AutoControl(myInventory.Object);

            // Assert
             Assert.ThrowsException<VINNotFoundException>(()=> findCar.FindCar("07xxxxxxxxxxxxxxx"));
          

            

            


        }

        [TestMethod]

        public void ReturnCorrectInstanceOf2_WhenLookingForCadillacMake()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            var findCar = new SUT.AutoControl(myInventory.Object);

            //Act
            var result = findCar.FindCarsByMake("Cadillac");

            //Assert

            Assert.IsTrue(result.Count.Equals(2));

        }
        [TestMethod]
        public void ReturnCorrectInstanceOf0_WhenLookingForAudiMake()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            var findCar = new SUT.AutoControl(myInventory.Object);

            //Act
            var result = findCar.FindCarsByMake("Audi");

            //Assert

            Assert.IsTrue(result.Count.Equals(0));

        }
        [TestMethod]
        public void ReturnProperluUpdate_WhenTheAddSuceceed()
        {
            
            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            var carAdded = new List<SUT.Auto>();
            carAdded.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "08xxxxxxxxxxxxxxx",
                Make = "Doudge",
                Year = "1968",
                Model = "Dart",
                LocationOnLot = "F9"
            });
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            myInventory.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);
            var findCar = new SUT.AutoControl(myInventory.Object);

            //Act
            var result = findCar.AddCar(carAdded[0]);

            //Assert

            Assert.AreEqual(7, result.Count());
     

        }

        [TestMethod]
        public void ThrowDuplicateVINException_WhencaronthelotwiththenewautosVIN()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            var carAdded = new List<SUT.Auto>();
            carAdded.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "03xxxxxxxxxxxxxxx",
                Make = "Cadillac",
                Year = "1963",
                Model = "Fleetwood",
                LocationOnLot = "A23"
            });
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            myInventory.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);
            
            //Act
            var findCar = new SUT.AutoControl(myInventory.Object);




            //Assert && Act

            Assert.ThrowsException<DuplicateVINException>(() => findCar.AddCar(carAdded[0]));







        }

        [TestMethod]
        public void ThrowVinDuplicateLocationException_whencaratthesamespotonthelotasthenewcar()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            var carAdded = new List<SUT.Auto>();
            carAdded.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "08xxxxxxxxxxxxxxx",
                Make = "Doudge",
                Year = "1968",
                Model = "Dart",
                LocationOnLot = "C7"
            });
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            myInventory.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);

            //Act
            var findCar = new SUT.AutoControl(myInventory.Object);




            // Assert 

            Assert.ThrowsException<DuplicateLocationException>(() => findCar.AddCar(carAdded[0]));







        }

        [TestMethod]
        public void ThrowInvalidVINException_whentheVINisnotexactly17characterslong ()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            var carAdded = new List<SUT.Auto>();
            carAdded.Add(new SUT.Auto
            {
                VehicleIdentificationNumber = "08xxxxxxxxxxxxxxxxx",
                Make = "Doudge",
                Year = "1968",
                Model = "Dart",
                LocationOnLot = "C7"
            });
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            myInventory.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);

            //Act
            var findCar = new SUT.AutoControl(myInventory.Object);

            //Assert

            Assert.ThrowsException<InvalidVINException>(() => findCar.AddCar(carAdded[0]));


        }



        [TestMethod]
        public void ReturnCollection_WhenCarIsRemoved()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            var carAdded = new List<SUT.Auto>();
            
            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            myInventory.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);
            var findCar = new SUT.AutoControl(myInventory.Object);

            //Act
            var result = findCar.RemoveCar("01xxxxxxxxxxxxxxx");

            //Assert

            Assert.AreEqual(5, result.Count());


        }

        [TestMethod]
        public void VINNotFoundException_WhenTheCartobeRemovedIsNotOntheLot()
        {

            //Arrange

            Mock<SUT.IAutoDBAccess> myInventory = new Mock<SUT.IAutoDBAccess>();
            var carAdded = new List<SUT.Auto>();

            myInventory.Setup(x => x.LoadLot()).Returns(AutoListTest());
            myInventory.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);

            //Act
            var findCar = new SUT.AutoControl(myInventory.Object);

         

            //Assert

            Assert.ThrowsException<VINNotFoundException>(() => findCar.RemoveCar("09xxxxxxxxxxxxxxx"));



        }




    }
}
