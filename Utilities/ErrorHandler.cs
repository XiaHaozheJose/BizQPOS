using System;
using System.Net;
using System.Windows;
using BizQPOS.Services;

namespace BizQPOS.Utilities
{
    public static class ErrorHandler
    {
        public static void HandleApiException(Exception ex)
        {
            if (ex is ApiException apiEx)
            {
                LogError(apiEx);

                MessageBox.Show(
                    $"操作失败: {apiEx.ErrorDetails?.Message ?? "未知错误"}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            else
            {
                LogError(ex);

                MessageBox.Show(
                    $"操作失败: {ex.Message}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private static void LogError(Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now}] 错误: {ex.Message}");
            if (ex is ApiException apiEx && apiEx.ErrorDetails != null)
            {
                Console.WriteLine($"代码: {apiEx.ErrorDetails.Code}, 子代码: {apiEx.ErrorDetails.SubCode}, 类型: {apiEx.ErrorDetails.Type}");
                Console.WriteLine($"详细信息: {apiEx.ErrorDetails.Message}");
            }
            else
            {
                Console.WriteLine($"堆栈信息: {ex.StackTrace}");
            }
        }
    }
}
