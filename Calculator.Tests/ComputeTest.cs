using NUnit.Framework;

namespace Calculator.Tests
{
    public class ComputeTest
    {
        [Test]
        public void Calculate_When_Invalid_Expression_Should_Return_Error_Message()
        {
            //Arrange
            var expression = "1 v 1";

            var result = "Invalid expression";

            //Act

            var sut = Common.Calculator.Calculate(expression);

            //Asert

            Assert.AreEqual(result, sut);
        }

        [Test]
        public void Calculate_When_divideByZero_Should_Return_Exception()
        {
            //Arrange
            var expression = "1/0";
                       
            //Asert

            Assert.Throws<System.DivideByZeroException>(() => Common.Calculator.Calculate(expression));
        }

        [Test]
        public void Calculate_Scenario_One_Should_Return_Success()
        {
            //Arrange
            var expression = "1 + 1";

            var result = "2";

            //Act

            var sut = Common.Calculator.Calculate(expression);

            //Asert

            Assert.AreEqual(result, sut);
        }

        [Test]
        public void Calculate_Scenario_Two_Should_Return_Success()
        {
            //Arrange
            var expression = "2-1 + 2";

            var result = "3";

            //Act

            var sut = Common.Calculator.Calculate(expression);

            //Asert

            Assert.AreEqual(result, sut);
        }


        [Test]
        public void Calculate_Scenario_Three_Should_Return_Success()
        {
            //Arrange
            var expression = "1*4 + 5 +2-3 + 6/8";

            var result = "35/4";

            //Act

            var sut = Common.Calculator.Calculate(expression);

            //Asert

            Assert.AreEqual(result, sut);
        }


        [Test]
        public void Calculate_Scenario_Four_Should_Return_Success()
        {
            //Arrange
            var expression = "7-8/6*6 + 25/4-20 /78*3";

            var result = "449/100";

            //Act

            var sut = Common.Calculator.Calculate(expression);

            //Asert

            Assert.AreEqual(result, sut);
        }
    }
}