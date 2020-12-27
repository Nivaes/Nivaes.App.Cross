namespace MvvmCross.UnitTest.Helpers
{
    using System;
    using MvvmCross.Tests;
    using Xunit;

    public class MvxUnitTestCommandHelperTests
    {
        [Fact]
        public void Helper_Can_Construct_Test()
        {
            var helper = new MvxUnitTestCommandHelper();
            Assert.IsType<MvxUnitTestCommandHelper>(helper);
        }

        [Fact]
        public void Can_Add_Item_To_Helper_Test()
        {
            var helper = new MvxUnitTestCommandHelper();

            var fakeCommand = new object { };

            helper.WillCallRaisePropertyChangedFor(fakeCommand);

            helper.RaiseCanExecuteChanged(fakeCommand);

            Assert.True(helper.HasCalledRaisePropertyChangedFor(fakeCommand));
        }

        [Fact]
        public void Testing_Unregistered_Item_Will_Throw_Test()
        {
            var helper = new MvxUnitTestCommandHelper();

            var fakeCommand = new object { };

            Action act = () => helper.HasCalledRaisePropertyChangedFor(fakeCommand);

            Assert.Throws<Exception>(act);
        }
    }
}
