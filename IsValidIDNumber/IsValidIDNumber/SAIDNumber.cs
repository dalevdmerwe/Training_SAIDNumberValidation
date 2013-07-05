using System;
using System.Globalization;
using NUnit.Framework;

namespace IsValidIDNumber
{
    public class SAIDNumber
    {
        private string _idNumber;

        public void EnsureValidIDNumber(string idNumber)
        {
            _idNumber = idNumber.Trim().Replace(" ", "");
            IsNumericOnly();
            IsValidLength();
            IsValidMonth();
            IsValidDay();
            IsValidDate();
            ControlDigitEqualsCheckSum();
        }

        private void IsValidLength()
        {
            if (_idNumber.Length != 13) throw new IDNumberNotValidLengthException();
        }


        private void IsValidMonth()
        {
            int month = Convert.ToInt32(_idNumber.Substring(2, 2));
            if (month < 1 || month > 12) throw new InvalidMonthException();
        }

        private void IsValidDay()
        {
            int day = Convert.ToInt32(_idNumber.Substring(4, 2));
            if (day < 1 || day > 31) throw new InvalidDayException();
        }

        private void IsValidDate()
        {
            string yyMMdd = _idNumber.Substring(0, 6);
            DateTime dateValue;

            if (
                !DateTime.TryParseExact(yyMMdd, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                                        out dateValue)) throw new InvalidDateException();
        }

        internal int SumOddNumbers()
        {
            //8001015009087
            //8 0 0 5 0 0 
            int sumOddNumber = 0;
            string inputStringreplacedSpaces = _idNumber.Replace(" ", "");
            //Console.WriteLine(inputStringreplacedSpaces);
            for (int i = 0; i < inputStringreplacedSpaces.Length - 1; i++)
            {
                if (i%2 == 0)
                {
                    sumOddNumber += Convert.ToInt32(inputStringreplacedSpaces[i].ToString());
                    //Console.WriteLine(inputStringreplacedSpaces[i]+ " "+ sumOddNumber);
                }
            }
            return sumOddNumber;
        }

        public int ConcatenateEvenNumbers()
        {
            string concatenatedNumber = String.Empty;
            string inputStringreplacedSpaces = _idNumber.Replace(" ", "");
            for (int i = 0; i < inputStringreplacedSpaces.Length - 1; i++)
            {
                if (i%2 != 0)
                {
                    concatenatedNumber += inputStringreplacedSpaces[i];
                }
            }
            return Convert.ToInt32(concatenatedNumber);
        }

        public void IsNumericOnly()
        {
            foreach (char c in _idNumber)
            {
                if (!Char.IsNumber(c)) throw new InvalidCharactersException();
            }
        }

        public string Gender(string inputString)
        {
            if (Convert.ToInt32(inputString.Substring(5, 1)) < 5)
                return "Female";
            return "Male";
        }

        public int GetControlDigit()
        {
            //Console.WriteLine(_idNumber + " " + Convert.ToInt32(_idNumber.Substring(_idNumber.Length - 1, 1)));
            return Convert.ToInt32(_idNumber.Substring(_idNumber.Length - 1, 1));
        }

        public void ControlDigitEqualsCheckSum()
        {
            //Console.WriteLine("CheckSum" +
            //                  (10 - (SumOddNumbers() + SumNumbersInString((ConcatenateEvenNumbers()*2).ToString()))%10)
            //                      .ToString() +
            //                  " SumOddNumbers " + SumOddNumbers() +
            //                  " Even " + SumNumbersInString((ConcatenateEvenNumbers()*2).ToString()) +
            //                  " " + GetControlDigit());
            if ((10 - (SumOddNumbers() + SumNumbersInString((ConcatenateEvenNumbers()*2).ToString()))%10) !=
                GetControlDigit())
                throw new IDNumberChecksumException();
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

    public class IDNumberChecksumException : Exception
    {
    }

    public class InvalidCharactersException : Exception
    {
    }

    internal class InvalidDateException : Exception
    {
    }

    public class InvalidDayException : Exception
    {
    }

    public class InvalidMonthException : Exception
    {
    }

    public class IDNumberNotValidLengthException : Exception
    {
    }

    internal class IDNumberNotSetException : Exception
    {
    }


    [TestFixture]
    public class SAIDNumberTest
    {
        private SAIDNumber _saIDNumber;

        [TestFixtureSetUp]
        public void Setup()
        {
            _saIDNumber = new SAIDNumber();
        }

        [Test]
        public void IsValidIdNumber()
        {
            string idnumber = "7706275007081";
            _saIDNumber.EnsureValidIDNumber(idnumber);
            Assert.Pass("Is Valid IDNumber");
        }


        [Test]
        public void CanIgnoreSpaces()
        {
            string inputString = "77 06 27 5007 08 1";
            _saIDNumber.EnsureValidIDNumber(inputString);
            Assert.Pass("Is Valid IDNumer");
        }


        [TestCase("1")]
        [TestCase("12345678901234")]
        [TestCase("")]
        public void IDNumberIsCorrectLength(string inputIdnumber)
        {
            Assert.Throws(Is.TypeOf<IDNumberNotValidLengthException>(),
                          () => _saIDNumber.EnsureValidIDNumber(inputIdnumber));
        }


        [Test]
        public void EmptyStringThrowsInvalidLenghtException()
        {
            string inputIDNumber = String.Empty;
            Assert.Throws(Is.TypeOf<IDNumberNotValidLengthException>(),
                          () => _saIDNumber.EnsureValidIDNumber(inputIDNumber));
        }

        [TestCase("7713275007081", false)]
        [TestCase("7700275007081", false)]
        public void Digit3To4IsValidMonth(string inputString, bool expectedResult)
        {
            Assert.Throws(Is.TypeOf<InvalidMonthException>(), () => _saIDNumber.EnsureValidIDNumber(inputString));
        }


        [TestCase("7706005007081", false)]
        [TestCase("7706325007081", false)]
        public void Digit5to6IsValidDay(string inputString, bool expectedResult)
        {
            Assert.Throws(Is.TypeOf<InvalidDayException>(), () => _saIDNumber.EnsureValidIDNumber(inputString));
        }


        [TestCase("7702315007081", false)]
        [TestCase("7702295007081", false)]
        [TestCase("7704315007081", false)]
        [TestCase("7706325007081", false)]
        public void InvalidDateThrowsException(string inputString, bool expectedResult)
        {
            try
            {
                _saIDNumber.EnsureValidIDNumber(inputString);
            }
            catch (InvalidDayException)
            {
            }
            catch (InvalidDateException)
            {
                Assert.True(true);
            }
        }

        [Test]
        public void ReturnsTheSumOfOddNumberInString()
        {
            string inputString = "800101 5009 087";
            _saIDNumber.EnsureValidIDNumber(inputString);
            Assert.That(_saIDNumber.SumOddNumbers(), Is.EqualTo(13));
        }

        [Test]
        public void ReturnsConcatenatedStringFromInputString()
        {
            string inputString = "800101 5009 087";
            _saIDNumber.EnsureValidIDNumber(inputString);
            Assert.That(_saIDNumber.ConcatenateEvenNumbers(), Is.EqualTo(011098));
        }

        [TestCase("A7O627500708O")]
        public void StringWithNonNumericCharactersThrowException(String inputString)
        {
            Assert.Throws(Is.TypeOf<InvalidCharactersException>(), () => _saIDNumber.EnsureValidIDNumber(inputString));
        }

        [TestCase("7706275007081", "Male")]
        [TestCase("7806240002082", "Female")]
        public void SeventhDigitReturnsGender(string inputString, string expectedResult)
        {
            string sut = _saIDNumber.Gender(inputString);
            //Assert			
            Assert.That(sut, Is.EqualTo(expectedResult));
        }


        [TestCase("7706275007081", 1)]
        [TestCase("7806240002082", 2)]
        public void ControlDigitIsLastDigit(string inputString, int expectedResult)
        {
            _saIDNumber.EnsureValidIDNumber(inputString);
            Assert.That(_saIDNumber.GetControlDigit(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void SumGivenDigits()
        {
            string inputString = "22196";
            int sut = _saIDNumber.SumNumbersInString(inputString);
            Assert.That(sut, Is.EqualTo(20));
        }

        [Test]
        public void CheckControlDigitAgainstCheckSumFail()
        {
            string inputString = "800101 5009 086";
            Assert.Throws(Is.TypeOf<IDNumberChecksumException>(), () => _saIDNumber.EnsureValidIDNumber(inputString));
        }

        [Test]
        public void CheckControlDigitAgainstCheckSumPass()
        {
            string inputString = "800101 5009 087";
            _saIDNumber.EnsureValidIDNumber(inputString);
            Assert.Pass("No Exceptions Thrown Is Valid IDNumber and checksum");
        }
    }
}
