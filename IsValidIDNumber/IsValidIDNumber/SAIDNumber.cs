using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IsValidIDNumber
{
    public class SAIDNumber
    {
        private string idnumber;

        public SAIDNumber(string inputidnumber)
        {
            idnumber = inputidnumber;
        }
        public bool IsValidLength(string idnumber)
        {
            if (idnumber.Length != 13)
                return false;
            return true;
        }

        public bool IsValid(string inputIdNumber)
        {
            string idnumbertrimmed = inputIdNumber.Trim();
            string idnumberReplaceStrings = idnumbertrimmed.Replace(" ", "");
            if (!IsValidLength(idnumberReplaceStrings) || 
                !IsValidMonth(idnumberReplaceStrings) || 
                !IsValidDate(idnumberReplaceStrings) || 
                !IsValidDay(idnumberReplaceStrings))
            return false;
            return true;
        }

        public bool IsValidMonth(string inputString)
        {
            int month = Convert.ToInt32(inputString.Substring(2, 2));
            if (month < 1 || month > 12 )
                return false;
            return true;
        }

        public bool IsValidDay(string inputString)
        {
            int day = Convert.ToInt32(inputString.Substring(4, 2));
            if (day < 1 || day > 31)
                return false;
            return true;
        }

        public bool IsValidDate(string inputString)
        {
            string yyMMdd = inputString.Substring(0, 6);
            DateTime dateValue;
            return (DateTime.TryParseExact(yyMMdd, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue));
        }

        public object SumOddNumbers(string inputString)
        {
            //8001015009087
            //8 0 0 5 0 0 
            //Console.WriteLine("TEST");
            int sumOddNumber = 0;
            string inputStringreplacedSpaces = inputString.Replace(" ", "");
            for (int i = 0; i < inputStringreplacedSpaces.Length-1; i++)
            {
                if (i%2 == 0)
                {
                    //sumOddNumber += Convert.ToInt32(inputStringreplacedSpaces[i]);
                    sumOddNumber += Convert.ToInt32(inputStringreplacedSpaces[i].ToString());
                    //Console.WriteLine(inputStringreplacedSpaces[i]+ " "+ sumOddNumber);
                }
            }
            return sumOddNumber;
        }

        public int ConcatenateEvenNumbers(string inputString)
        {
            string concatenatedNumber = String.Empty;
            string inputStringreplacedSpaces = inputString.Replace(" ", "");
            for (int i = 0; i < inputStringreplacedSpaces.Length - 1; i++)
            {
                if (i % 2 != 0)
                {
                    concatenatedNumber += inputStringreplacedSpaces[i];
                }
            }
            return Convert.ToInt32(concatenatedNumber);
        }

        public bool IsNumericOnly(string inputString)
        {
            bool isnumeric = true;
            foreach (char c in inputString)
            {
                if (!Char.IsNumber(c))
                    isnumeric = false;
            }
            return isnumeric;
        }

        public string Gender(string inputString)
        {
            if (Convert.ToInt32(inputString.Substring(5, 1)) < 5)
                return "Female";
            return "Male";
        }

        public int ControlDigit(string inputString)
        {
            return Convert.ToInt32(inputString.Substring(inputString.Length-1,1));
        }

        public bool ControlDigitEqualsCheckSum(string inputString)
        {
            //int controlDigit = idnumber.ControlDigit();
            return true;
        }

        public int SumNumbersInString(string inputString)
        {
            int sum = 0;
            foreach (char c in inputString)
            {
                sum += Convert.ToInt32(c.ToString());
            }
            return sum;
        }
    }

    [TestFixture]
    public class SAIDNumberTest
    {

        [Test]
        public void Has13Digits()
        {
            //Arrange
            var idnumber = "1234567890123";
            //Act			
            var sut = new SAIDNumber().IsValidLength(idnumber);
            //Assert			
            Assert.That(sut,Is.EqualTo(true));
        }


        [Test]
        public void NumericCharactersOnly()
        {
            //Arrange
            //Act			
            //Assert			
        }




        [TestCase("1",false)]
        [TestCase("12345678901234", false)]
        public void MoreThenThirteenAndLessThenThirteenReturnsFalse(string inputIdnumber, bool result)
        {
            var sut = new SAIDNumber().IsValidLength(inputIdnumber);
            //Assert			
            Assert.That(sut,Is.EqualTo(result));
        }


        [Test]
        public void emtyStringReturnsFalse()
        {
            var inputIDNumber = String.Empty;
            //Act			
            var sut = new SAIDNumber().IsValid(inputIDNumber);
            //Assert			
            Assert.That(sut, Is.EqualTo(false));
        }


        [TestCase("7706275007081",true)]
        [TestCase("7713275007081", false)]
        [TestCase("7700275007081", false)]
        public void Digit3To4IsValidMonth(string inputString, bool expectedResult)
        {
            //Act			
            var sut = new SAIDNumber().IsValidMonth(inputString);
            //Assert			
            Assert.That(sut,Is.EqualTo(expectedResult));
        }


        [TestCase("7706275007081",true)]
        [TestCase("7706005007081", false)]
        [TestCase("7706325007081", false)]
        public void Digit5to6IsValidDay(string inputString, bool expectedResult)
        {
            var sut = new SAIDNumber().IsValidDay(inputString);
            //Assert			
            Assert.That(sut, Is.EqualTo(expectedResult));
        }

        [TestCase("7702315007081", false)]
        [TestCase("7702295007081", false)]
        [TestCase("7704315007081", false)]
        [TestCase("7706325007081", false)]
        public void IsValidDate(string inputString, bool expectedResult)
        {
            var sut = new SAIDNumber().IsValidDate(inputString);
            //Assert			
            Assert.That(sut, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IgnoresSpaces()
        {
            var idnumber = "77 06 27 5007 08 1";
            //Act			
            var sut = new SAIDNumber().IsValid(idnumber);
            //Assert			
            Assert.That(sut, Is.EqualTo(true));
        }


        [Test]
        public void SumOfOddNumbers()
        {
            //Arrange
            var inputString = "800101 5009 087";
            //Act			
            var sut = new SAIDNumber().SumOddNumbers(inputString);
            //Assert			
            Assert.That(sut, Is.EqualTo(13));
        }

        [Test]
        public void ConcatenateEvenNumbers()
        {
            //Arrange
            var inputString = "800101 5009 087";
            //Act			
            var sut = new SAIDNumber().ConcatenateEvenNumbers(inputString);
            //Assert			
            Assert.That(sut, Is.EqualTo(011098));
        }


        [TestCase("77O6275OO7081", false)]
        [TestCase("7706275007081", true)]
        public void IsNumericOnly(String inputString, bool expectedResult)
        {
            var sut = new SAIDNumber().IsNumericOnly(inputString);
            //Assert			
            Assert.That(sut, Is.EqualTo(expectedResult));
        }


        [TestCase("7706275007081", "Male")]
        [TestCase("7806240002082", "Female")]
        public void SeventhDigitReturnsGender(string inputString, string expectedResult)
        {
            var sut = new SAIDNumber().Gender(inputString);
            //Assert			
            Assert.That(sut,Is.EqualTo(expectedResult));
        }


        [TestCase("7706275007081", 1)]
        [TestCase("7806240002082", 2)]
        public void ControlDigitIsLastDigit(string inputString,int expectedResult)
        {
            var sut = new SAIDNumber().ControlDigit(inputString);
            Assert.That(sut, Is.EqualTo(expectedResult));
        }


        [TestCase("7706275007081", true)]
        [TestCase("7806240002082", true)]
        public void CheckControlDigitAgainstCheckSum(SAIDNumber idnumber, bool expectedResult)
        {
            var sut = new SAIDNumber().ControlDigitEqualsCheckSum(idnumber);
            Assert.That(sut, Is.EqualTo(expectedResult));
        }


        [Test]
        public void SumGivenDigits()
        {
            //Arrange
            var inputString = "22196";
            //Act			
            var sut = new SAIDNumber().SumNumbersInString(inputString);
            //Assert			
            Assert.That(sut,Is.EqualTo(20));
        }

    }
}
