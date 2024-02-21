Feature: Homework
	Simple calculator for adding two numbers

Scenario: 環境測試
When 環境測試

Scenario: 作业1-打印登录Token
Given 存在用户名为"joseph"和密码为"123"的用户
When 通过API以用户名为"joseph"和密码为"123"登录时
Then 打印Token
#
Scenario: 作业2-操作浏览器
When 在谷歌搜索关键字"cucumber"
Then 打印谷歌为您找到的相关结果数