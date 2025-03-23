using PureLifeClinic.API.Attributes;
using PureLifeClinic.Core.Enums;
using Xunit;

namespace PeruLifeClinic.Api.Tests.Attributes
{
    public class PermissionAuthorizeAttributeTests
    {
        public PermissionAuthorizeAttributeTests() { }

        [Fact]
        public void Constructor_WithMultiplePermissions_SetsPolicyCorrectly()
        {
            // Arrange
            string resource = "Customer";
            PermissionOperator permissionOperator = PermissionOperator.And;
            PermissionAction[] permissions = { PermissionAction.View, PermissionAction.CreateDelete };

            // Act
            var attribute = new PermissionAuthorizeAttribute(resource, permissionOperator, permissions);

            // Assert
            string expectedPolicy = "PERMISSION_Customer_1_1_2";
            Assert.Equal(expectedPolicy, attribute.Policy);
        }

        [Fact]
        public void Constructor_WithSinglePermission_SetsPolicyCorrectly()
        {
            // Arrange
            string resource = "Customer";
            PermissionAction permission = PermissionAction.View;

            // Act
            var attribute = new PermissionAuthorizeAttribute(resource, permission);

            // Assert
            string expectedPolicy = "PERMISSION_Customer_1_1";
            Assert.Equal(expectedPolicy, attribute.Policy);
        }

        [Theory]
        [InlineData("PERMISSION_Customer_1_1_2", PermissionOperator.And)]
        [InlineData("PERMISSION_Product_2_3_4", PermissionOperator.Or)]
        public void GetOperatorFromPolicy_ReturnsCorrectOperator(string policyName, PermissionOperator expectedOperator)
        {
            // Act
            var result = PermissionAuthorizeAttribute.GetOperatorFromPolicy(policyName);

            // Assert
            Assert.Equal(expectedOperator, result);
        }

        [Theory]
        [InlineData("PERMISSION_Customer_1_1_2", "Customer")]
        [InlineData("PERMISSION_Product_2_3_4", "Product")]
        public void GetResourceFromPolicy_ReturnsCorrectResource(string policyName, string expectedResource)
        {
            // Act
            var result = PermissionAuthorizeAttribute.GetResourceFromPolicy(policyName);

            // Assert
            Assert.Equal(expectedResource, result);
        }

        [Theory]
        [InlineData("PERMISSION_Customer_1_1_2", new int[] { 1, 2 })]
        [InlineData("PERMISSION_Product_2_3_4", new int[] { 3, 4 })]
        public void GetPermissionsFromPolicy_ReturnsCorrectPermissions(string policyName, int[] expectedPermissions)
        {
            // Act
            var result = PermissionAuthorizeAttribute.GetPermissionsFromPolicy(policyName);

            // Assert
            Assert.Equal(expectedPermissions, result);
        }
    }
}
