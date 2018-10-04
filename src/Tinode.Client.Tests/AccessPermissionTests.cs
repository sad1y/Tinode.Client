using System;
using Tinode.Client.Extensions;
using Xunit;

namespace Tinode.Client.Tests
{
    public class AccessPermissionTests
    {
        [Theory]
        [InlineData(AccessPermission.No, "N")]
        [InlineData(AccessPermission.Read, "R")]
        [InlineData(AccessPermission.Join | AccessPermission.Owner, "JO")]
        [InlineData(AccessPermission.Read | AccessPermission.Approve | AccessPermission.Delete, "RAD")]
        [InlineData(AccessPermission.Join | AccessPermission.Write | AccessPermission.Sharing | AccessPermission.Approve, "JWAS")]
        [InlineData(AccessPermission.Join | AccessPermission.Write | AccessPermission.Sharing | AccessPermission.Approve |
                    AccessPermission.Delete, "JWASD")]
        [InlineData(AccessPermission.Join | AccessPermission.Write | AccessPermission.Read | AccessPermission.Sharing | AccessPermission.Approve |
                    AccessPermission.Delete, "JRWASD")]
        [InlineData(AccessPermission.Join | AccessPermission.Write | AccessPermission.Read | AccessPermission.Presence | AccessPermission.Sharing |
                    AccessPermission.Approve |
                    AccessPermission.Delete, "JRWPASD")]
        [InlineData((AccessPermission) 255, "JRWPASDO")]
        public void AsString(AccessPermission acs, string expectedResult)
        {
            var str = acs.AsString();
            Assert.Equal(expectedResult, str);
        }


        [Theory]
        [InlineData("", AccessPermission.No)]
        [InlineData(null, AccessPermission.No)]
        [InlineData("R", AccessPermission.Read)]
        [InlineData("JO", AccessPermission.Join | AccessPermission.Owner)]
        [InlineData("RAD", AccessPermission.Read | AccessPermission.Approve | AccessPermission.Delete)]
        [InlineData("JWAS", AccessPermission.Join | AccessPermission.Write | AccessPermission.Sharing | AccessPermission.Approve)]
        [InlineData("JWASD", AccessPermission.Join | AccessPermission.Write | AccessPermission.Sharing | AccessPermission.Approve |
                             AccessPermission.Delete)]
        [InlineData("JRWASD", AccessPermission.Join | AccessPermission.Write | AccessPermission.Read | AccessPermission.Sharing |
                              AccessPermission.Approve |
                              AccessPermission.Delete)]
        [InlineData("JRWPASD", AccessPermission.Join | AccessPermission.Write | AccessPermission.Read | AccessPermission.Presence |
                               AccessPermission.Sharing | AccessPermission.Approve |
                               AccessPermission.Delete)]
        [InlineData("JRWPASDO", (AccessPermission) 255)]
        public void FromString(string str, AccessPermission expectedResult)
        {
            var acs = str.AsAccessPermission();
            Assert.Equal(expectedResult, acs);
        }

        [Fact]
        public void FromString_ShouldThrowExceptionOnUnkownChar()
        {
            var str = "INVALID";

            Assert.Throws<ArgumentException>(() => str.AsAccessPermission());
        }
    }
}