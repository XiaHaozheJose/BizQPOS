using Models;

namespace BizQPOS.Models
{
    internal class GlobalState
    {
        // 当前操作的用户信息
        public static OperatorResponse? CurrentOperator { get; private set; }
        public static bool IsShop { get; private set; }
        public static UserEntity? CurrentUser { get; private set; }

        // 设置当前操作用户
        public static void SetCurrentOperator(OperatorResponse response)
        {
            var userOperator = response.Operator.payload.Data;
            if (userOperator != null)
            {
                CurrentUser = userOperator;
            }
            IsShop = response.Operator.type == Operator.UserType.Shop;
        }

        // 清除当前用户信息（如在退出登录时）
        public static void ClearCurrentOperator()
        {
            CurrentOperator = null;
        }

        // 判断是否已登录
        public static bool IsLoggedIn => CurrentOperator != null;
    }
}
