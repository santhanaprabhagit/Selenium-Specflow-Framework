Feature:  Automation exercise website login

Background: Login to automation exercise website and validate page name
  Given User is login in using 'Testuser' account

@login @Sanity
Scenario: User login and logout
  When User clicks on the logout link
  Then User see the login page  