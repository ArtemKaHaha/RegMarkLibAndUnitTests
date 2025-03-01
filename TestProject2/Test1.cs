using REG_MARK_LIB;

namespace TestProject2
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void CheckMark_TooLongPlate_ReturnsFalse()
        {
            Assert.IsFalse(Class1.CheckMark("A123AA1234"));
        }

        [TestMethod]
        public void CheckMark_TooShortPlate_ReturnsFalse()
        {
            Assert.IsFalse(Class1.CheckMark("A123AA12"));
        }

        [TestMethod]
        public void CheckMark_OnlyNumbers_ReturnsFalse()
        {
            Assert.IsFalse(Class1.CheckMark("123456789"));
        }

        [TestMethod]
        public void GetNextMarkAfterInRange_OutOfStock_ReturnsOutOfStock()
        {
            // Arrange
            string prevMark = "A999AB999";
            string rangeStart = "A999AA000";
            string rangeEnd = "A999AB000";

            // Act
            string result = Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);

            // Assert
            Assert.AreEqual("out of stock", result);
        }

        [TestMethod]
        public void GetNextMarkAfter_ValidMark_ReturnsNextMark()
        {
            // Arrange
            string mark = "A000AA000";

            // Act
            string result = Class1.GetNextMarkAfter(mark);

            // Assert
            Assert.AreEqual("A001AA000", result);
        }

        [TestMethod]
        public void GetNextMarkAfter_IncrementingNumber_CorrectlyIncrements()
        {
            // Arrange
            string mark = "A001AA000";

            // Act
            string result = Class1.GetNextMarkAfter(mark);

            // Assert
            Assert.AreEqual("A002AA000", result);
        }

        [TestMethod]
        public void GetNextMarkAfterInRange_InvalidRangeEnd_ThrowsArgumentException()
        {
            // Arrange
            string prevMark = "A999AA999";
            string rangeStart = "A999AA000";
            string rangeEnd = "INVALID"; // Неверный формат

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd));
        }

        [TestMethod]
        public void TestValidInput()
        {
            string prevMark = "A001AA001";
            string rangeStart = "A001AA000";
            string rangeEnd = "A002AA999";

            string result = Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);
            Console.WriteLine(result); // Ожидается: A002AA001
        }

        [TestMethod]
        public void TestOutOfStock()
        {
            string prevMark = "A002AA999";
            string rangeStart = "A001AA000";
            string rangeEnd = "A002AA999";

            string result = Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);
            Console.WriteLine(result); // Ожидается: out of stock
        }

        [TestMethod]
        public void TestInvalidPrevMark()
        {
            string prevMark = "INVALID";
            string rangeStart = "A001AA000";
            string rangeEnd = "A002AA999";

            try
            {
                Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message); // Ожидается: Некорректный формат номерного знака или диапазона.
            }
        }

        [TestMethod]
        public void TestInvalidRangeStart()
        {
            string prevMark = "A001AA001";
            string rangeStart = "INVALID";
            string rangeEnd = "A002AA999";

            try
            {
                Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message); // Ожидается: Некорректный формат номерного знака или диапазона.
            }
        }

        [TestMethod]
        public void TestInvalidRangeEnd()
        {
            string prevMark = "A001AA001";
            string rangeStart = "A001AA000";
            string rangeEnd = "INVALID";

            try
            {
                Class1.GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message); // Ожидается: Некорректный формат номерного знака или диапазона.
            }
        }

        [TestMethod]
        public void TestNonSequentialMarks()
        {
            Assert.ThrowsException<ArgumentException>(() => Class1.GetCombinationsCountInRange("A000AB001", "A000AA999"));
        }

        [TestMethod]
        public void GetCombinationsCountInRange_EmptyMark2_ThrowsArgumentException()
        {
            // Arrange
            string mark1 = "A000AA000";
            string mark2 = "";

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => Class1.GetCombinationsCountInRange(mark1, mark2));
        }

        [TestMethod]
        public void GetCombinationsCountInRange_InvalidMarkFormat_ThrowsArgumentException()
        {
            // Arrange
            string mark1 = "INVALID";
            string mark2 = "A000AA005";

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => Class1.GetCombinationsCountInRange(mark1, mark2));
        }
    }
}
