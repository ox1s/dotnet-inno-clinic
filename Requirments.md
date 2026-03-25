# Task Implement Authorization API

### US-1 Sign Up

_As a PATIENT_
_I want to sign up_
_so as I can make an appointment with doctor and check info in personal page_

#### Preconditions

- User isn’t signed in

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                           |
| ----- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                           |
| AC-1  | When User clicks “Sign up” link in the modal window of signing in <br><br>Then the Sign up modal window is displayed                                                                                                                                                                      |
| AC-2  | The Sign up modal window has the following attributes:<br><br>- e-mail - email text required field<br>- password - password text required field<br>- re-entered password - password text required field<br>- button “Sign up”<br>- link “Sign in”<br>- "x" button in the top right corner |
| AC-3  | The entered e-mail has to be unique                                                                                                                                                                                                                                                       |
| AC-4  | The password and repeated password must coincide                                                                                                                                                                                                                                          |
| AC-5  | When the User clicks the “Sign up” button <br><br>Given the entered data is VALID<br><br>Then the system should send an email with a link to the entered email address to confirm signing up.                                                                                             |
| AC-5  | When the User clicks the “Sign up” button <br><br>Given the entered email isn’t unique<br><br>Then the system should display the notification “Someone already uses this email”.                                                                                                          |
| AC-6  | When the User clicks the “Sign in” link <br><br>Then the Sign In modal window must be displayed                                                                                                                                                                                           |
| AC-7  | Given at least one of the fields is empty<br><br>Or at least one of the fields is invalid<br><br>Then the button “Sign up” is disabled                                                                                                                                                    |
| AC-8  | When User clicks "x" button in the top right corner<br><br>Then the modal window should be closed                                                                                                                                                                                         |

#### Fields description

|       |                     |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| ----- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Field name**      | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| F-1   | E-mail              | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email exists in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email already exists”                                   |
| F-2   | Password            | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, enter the password”                                                                                                                                                                                                                                                                             |
| F-3   | Re-entered password | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Label:** Repeat entered password<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, reenter the password”<br>- When the re-entered password doesn’t coincide with the password And the field loses focusThen the border of the field becomes red And the error message is shown to the User “The passwords you’ve entered don’t coincide” |

### US-2 Sign in

_As a PATIENT_

_I want to sign in_

_so as I can make an appointment with doctor and check info in personal page_

#### Preconditions

- User isn’t signed in

#### Acceptance criteria

|       |                                                                                                                                                                                                                                   |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                   |
| AC-1  | When the User clicks button/icon to sign in <br><br>OR the User clicks button “Confirm an appointment” <br><br>Then a modal window must be displayed                                                                              |
| AC-2  | The modal window must contain the following attributes<br><br>- e-mail - email text required field<br>- password - password text required field<br>- button “Sign in”<br>- link “Sign up”<br>- "x" button in the top right corner |
| AC-3  | When the User clicks the “Sign in” button <br><br>Then the system must check if this account exists in the system                                                                                                                 |
| AC-4  | Given the account exists in the system <br><br>Then the system should display a notification "You've signed in successfully"                                                                                                      |
| AC-5  | Given the account doesn’t exist in the system <br><br>Then the system should display a notification “Either an email or a password is incorrect”                                                                                  |
| AC-6  | When a User clicks the “Sign up” link <br><br>Then the modal window to sign up must be displayed                                                                                                                                  |
| AC-7  | When User clicks "x" button in the top right corner<br><br>Then the modal window should be closed                                                                                                                                 |
| AC-8  | Given at least one of the fields is empty<br><br>Or at least one of the fields is invalid<br><br>Then the button “Sign in” is disabled                                                                                            |

#### Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| F-1   | E-mail         | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email doesn’t exist in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email doesn’t exist” |
| F-2   | Password       | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, enter the password”                                                                                                                                                                                                                                                 |

### US-3 Sign out

_As a PATIENT_

_I want to sign out_

_so as I can change user or end work with system_

#### Preconditions

- User is signed in

#### Acceptance criteria

|       |                                                                      |
| ----- | -------------------------------------------------------------------- |
| **#** | **Description**                                                      |
| AC-1  | When a User clicks “Sign out” button Then user token must be deleted |

### US-34 Sign in as a worker

_As a DOCTOR, RECEPTIONIST_

_I want to sign in_

_so as I can do my work using the system_

#### Preconditions

- User isn’t signed in
- User opened the system/application

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                                |
| AC-1  | When the User opens the systemThen the modal window to sign in is displayed                                                                                                                                                                                                                                    |
| AC-2  | The modal window must contain the following attributes<br><br>- e-mail - email text required field<br>- password - password text required field<br>- button “Sign in”                                                                                                                                          |
| AC-3  | When the User clicks the “Sign in” button <br><br>Then the system must check if this account exists in the system                                                                                                                                                                                              |
| AC-4  | Given the account exists in the system  <br>And the profile linked to the account has status “At work” or “On vacation”, “Sick Day”, “Sick Leave”, “Self-isolation”, “Leave without pay”<br><br>Then the system should display a notification "You've signed in successfully"<br><br>And display the home page |
| AC-5  | Given the account doesn’t exist in the system <br><br>Or the profile linked to the account has status “Inactive”<br><br>Then the system should display a notification “Either an email or a password is incorrect”                                                                                             |
| AC-6  | User can’t use the system while not signed in                                                                                                                                                                                                                                                                  |

#### Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| F-1   | E-mail         | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email doesn’t exist in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email doesn’t exist” |
| F-2   | Password       | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, enter the password”                                                                                                                                                                                                                                                 |

### US-35 Sign out as a worker

_As a DOCTOR, RECEPTIONIST_
_I want to sign out_
_so as I can end work with system_

#### Preconditions

- User is signed in

#### Acceptance criteria

|       |                                                                                                                          |
| ----- | ------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                          |
| AC-1  | When a User clicks “Sign out” button Then user token must be deleted<br><br>And the modal window to sign in is displayed |

# Task Implement Offices API

### US-29 View offices

_As a RECEPTIONIST_
_I want to view offices_
_so as I can have a look at all the offices of clinic_

#### Preconditions

- Receptionist is signed in

#### Acceptance criteria

|       |                                                                                                                                                |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                |
| AC-1  | When the Receptionist clicks on “Offices” menu item<br><br>Then the page with the table of the offices is displayed                            |
| AC-2  | The table should contain the following fields<br><br>- office address<br>- status - radiobuttons (Active, Inactive)<br>- registry phone number |
| AC-3  | The page should contain the “Create office” button to create new office                                                                        |

### US-30 View office information

_As a RECEPTIONIST_
_I want to view office information_
*so as I can view office details and edit if necessary* 

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Offices” page

#### Acceptance criteria

|       |                                                                                                                                                                                                    |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                    |
| AC-1  | When the Receptionist clicks on the row of the office<br><br>Then the page with the office information is displayed                                                                                |
| AC-2  | Personal information page consists of the fields with the following information below:<br><br>- photo<br>- office address<br>- status - radiobuttons (Active, Inactive)<br>- registry phone number |
| AC-3  | The page should contain “Edit” button to edit information                                                                                                                                          |

### US-31 Create office

_As a RECEPTIONIST_
_I want to create office_
_so as new offices of clinic become able to include in the system_

#### Preconditions

- Receptionist is signed in
- Receptionist is on page “Offices”

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                                                                                                                                         |     |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --- |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                         |     |
| AC-1  | When the Receptionist clicks “Create office” button <br><br>Then the modal window/page for creation is displayed                                                                                                                                                                                                                                                                                                                                        |     |
| AC-2  | The modal window/page for the office’s profile creation must contain the following fields:<br><br>- photo - attached file<br>- city - text required field<br>- street - text required field<br>- house number - text required field<br>- office number - text required field<br>- registry phone number - text required field<br>- status - radiobuttons (Active, Inactive)<br><br>And 2 footer buttons:<br><br>- “Confirm” button<br>- “Cancel” button |     |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled                                                                                                                                                                                                                                                                                                |     |
| AC-4  | Given all of the required fields are completed<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Office to the system                                                                                                                                                                                                                                                                                      |     |
| AC-5  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                                                                                                                                                                                                                                           |     |
| AC-6  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                                                                                                                                                                                            |     |
| AC-7  | When the User clicks “Yes” button<br><br>Then the dialog window is closed<br><br>And the page “Offices” is displayed                                                                                                                                                                                                                                                                                                                                    |     |
| AC-8  | When the User clicks button “No”<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields                                                                                                                                                                                                                                                                                                             |     |
| AC-9  | Office’s address is formed from City, Street, House number, Office number                                                                                                                                                                                                                                                                                                                                                                               |     |

#### Fields description

|       |                       |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| ----- | --------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**        | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-1   | Photo                 | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-2   | City                  | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s city”                                                                                                                                                                                                                                                                              |
| F-3   | Street                | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s street”                                                                                                                                                                                                                                                                            |
| F-4   | House number          | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s house number”                                                                                                                                                                                                                                                                      |
| F-5   | Office number         | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s number”                                                                                                                                                                                                                                                                             |
| F-6   | Registry phone number | **Required:** yes<br><br>**Type:** number input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Phone number field contains + prefix that cannot be deleted<br>- Given the field contains non-numeric symbolsAnd the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid phone number”<br>- Given the phone number field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the phone number” |
| F-7   | Status                | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive”<br><br>**Default value:** Active                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |

### US-32 Change office’s status

_As a RECEPTIONIST_
_I want to change office’s status_
_so as I can mark office that is not working anymore_

#### Preconditions

- Receptionist is signed in
- Receptionist is on “Offices” page

#### Acceptance criteria

|       |                                                                                                                                       |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                       |
| AC-1  | When the User changes the status of the office<br><br>Then the system changes status of this office in the database                   |
| AC-2  | When the User changes status to “Inactive”<br><br>Then all the doctors and receptionists related to this office get status “Inactive” |

#### Fields description

|       |                |                                                                                         |
| ----- | -------------- | --------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                         |
| F-1   | Status         | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive” |

### US-33 Edit office

_As a RECEPTIONIST_
_I want to edit office information_
_so as I can correct some mistakes in it_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Office information” page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                              |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                              |
| AC-1  | When the User clicks on “Edit” button<br><br>Then all the fields on the profile page become editable                                                                                                                                                                         |
| AC-2  | 2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                                |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the “Save changes” button is disabled                                                                                                                |
| AC-4  | Given all of the required fields are completed<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the profile information of this Office in the system<br><br>And the page to view office information is displayed with updated data |
| AC-5  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                                     |
| AC-6  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                 |
| AC-7  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved                                                                                                              |
| AC-8  | When the User clicks “No” button <br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields                                                                                                             |

#### Fields description

|       |                       |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| ----- | --------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**        | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-1   | Photo                 | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-2   | City                  | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s city”                                                                                                                                                                                                                                                                              |
| F-3   | Street                | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s street”                                                                                                                                                                                                                                                                            |
| F-4   | House number          | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s house number”                                                                                                                                                                                                                                                                      |
| F-5   | Office number         | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the address field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the office’s number”                                                                                                                                                                                                                                                                             |
| F-6   | Registry phone number | **Required:** yes<br><br>**Type:** number input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Phone number field contains + prefix that cannot be deleted<br>- Given the field contains non-numeric symbolsAnd the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid phone number”<br>- Given the phone number field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the phone number” |
| F-7   | Status                | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |

# Task Implement Services API

### US-5 View services

_As a PATIENT_
_I want to view services_
_so as I can investigate all the services of clinic and after that choose appropriate one_

#### Acceptance criteria

|       |                                                                                                                    |
| ----- | ------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                    |
| AC-1  | When a User clicks on text/icon to move to the services page<br><br>Then the page with services is displayed       |
| AC-2  | The page should contain 3 tabs:<br><br>- consultations<br>- diagnostics<br>- analyses                              |
| AC-3  | Consultations tab is displayed by default                                                                          |
| AC-4  | Given a User is on Consultations tab <br><br>Then the list of consultations grouped by specialization is displayed |
| AC-5  | Given a User is on Diagnostics tab <br><br>Then the list of diagnostic services is displayed                       |
| AC-6  | Given a User is on Analyzes tab <br><br>Then the list of analyzes is displayed                                     |
| AC-7  | Tabs should contain only specializations and services that have “Active” status                                    |

### US-36 Create specialization

_As a RECEPTIONIST_
_I want to create specialization_
_so as new specializations of clinic is included  in the system_

#### Preconditions

- Receptionist is signed in
- Receptionist is on page “Specializations”

#### Acceptance criteria

|       |                                                                                                                                                                                                                                |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                |
| AC-1  | When the Receptionist clicks “Create specialization” button <br><br>Then the modal window/page for creation is displayed                                                                                                       |
| AC-2  | The modal window/page for the specialization creation must contain a field for entering the name of specialization, a field for status selecting, the table of services and two footer buttons: “Confirm” and “Cancel”         |
| AC-3  | The table of services should contain the following fields<br><br>- service name<br>- price<br>- status - radiobuttons (Active, Inactive)<br>- service’s category name                                                          |
| AC-4  | Given the name of specialization is completed<br><br>And table of services  contains minimum 1 row<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Specialization to the system |
| AC-5  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                  |
| AC-6  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                   |
| AC-7  | When the User clicks “Yes” button<br><br>Then the dialog window is closed<br><br>And the page “Specializations” is displayed                                                                                                   |
| AC-8  | When the User clicks “No” button<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields                                                                                    |
| AC-9  | Table of services should contain “Add service” button to add new service (new row)                                                                                                                                             |
| AC-10 | Given the name of specialization is empty Or the table is empty<br><br>Then the button “Confirm” is disabled                                                                                                                   |

#### Fields description

|       |                |                                                                                                                                                                                                                                                                                                    |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                    |
| F-1   | Name           | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:**<br><br>- Given the name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the name” |
| F-2   | Status         | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive”<br><br>**Default value:** Active                                                                                                                                                                           |

### US-37 Change specialization’s status

_As a RECEPTIONIST_
_I want to change specialization’s status_
_so as I can mark specialization that is not available anymore_

#### Preconditions

- Receptionist is signed in
- Receptionist is on “Specializations” page

#### Acceptance criteria

|       |                                                                                                                                          |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                          |
| AC-1  | When the User changes the status of the specialization<br><br>Then the system changes status of this specialization in the database      |
| AC-2  | When the User changes status to “Inactive”<br><br>Then all the doctors and services related to this specialization get status “Inactive” |

#### Fields description

|       |                |                                                                                         |
| ----- | -------------- | --------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                         |
| F-1   | Status         | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive” |

### US-38 Edit specialization

_As a RECEPTIONIST_
_I want to edit specialization information_
_so as I can correct some mistakes in it_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Specialization information” page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                          |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                          |
| AC-1  | When the User clicks on “Edit” button<br><br>Then the field for entering the name of specialization is editable<br><br>And the table of services should contain “Add service” button to add new service (new row)                                                                                        |
| AC-2  | 2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                                                            |
| AC-3  | Given the name of specialization is emptyOr table is empty<br><br>Then the “Save changes” button is disabled                                                                                                                                                                                             |
| AC-4  | Given the name of specialization is completedAnd table isn’t empty<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the information of this Specialization in the system<br><br>And the page to view specialization information is displayed with updated data |
| AC-5  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                                                                 |
| AC-6  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                                             |
| AC-7  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved                                                                                                                                          |
| AC-8  | When the User clicks “No” button <br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields                                                                                                                                         |

#### Fields description

|       |                |                                                                                                                                                                                                                                                                                                    |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                    |
| F-1   | Name           | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:**<br><br>- Given the name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the name” |
| F-2   | Status         | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive”                                                                                                                                                                                                            |
|       |                |                                                                                                                                                                                                                                                                                                    |

### US-39 View specialization’s info

_As a RECEPTIONIST_
_I want to view specialization’s info_
_so as I can view all the specialization information_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Specializations” page

#### Acceptance criteria

|       |                                                                                                                                                                       |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                       |
| AC-1  | When the Receptionist clicks on the row of the specialization list<br><br>Then the specialization page with the whole information is displayed                        |
| AC-2  | Specialization’s information page contains the name of specialization, the specialization’s status, the table of services related to this specialization              |
| AC-3  | The table of services should contain the following fields<br><br>- service name<br>- price<br>- status - radiobuttons (Active, Inactive)<br>- service’s category name |
| AC-4  | The page should contain “Edit” button to edit specialization information                                                                                              |

### US-40 View specializations list

_As a RECEPTIONIST_
_I want to view specializations_
_so as I can have a look at all the specializations of clinic_

#### Preconditions

- Receptionist is signed in

#### Acceptance criteria

|       |                                                                                                                                     |
| ----- | ----------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                     |
| AC-1  | When the Receptionist clicks on “Specializations” menu item<br><br>Then the page with the table of the specializations is displayed |
| AC-2  | The table should contain the following fields<br><br>- specialization name<br>- status radiobuttons (active or not active)          |
| AC-3  | The page should contain the “Create specialization” button to create specialization                                                 |

### US-41 Create service

_As a RECEPTIONIST_
_I want to create service_
_so as new services of clinic become able to include in the system_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the modal window/page for creation of specialization or on “Specialization information” page in Edit mode

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                      |
| AC-1  | When the Receptionist clicks “Add service” button <br><br>Then the modal window/page for creation is displayed                                                                                                                                                                                                                       |
| AC-2  | The modal window/page for the service’s creation must contain the following fields:<br><br>- service name - text required field<br>- price -  numeric text required field<br>- status - radiobuttons (Active, Inactive)<br>- service category - dropdown<br><br>and 2 footer buttons:<br><br>- “Confirm” button<br>- “Cancel” button |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled                                                                                                                                                                             |
| AC-4  | Given all of the required fields are completed<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Service to the system                                                                                                                                                                  |
| AC-5  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                                                                                                                        |
| AC-6  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                                                                         |
| AC-7  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the previous page is displayed                                                                                                                                                                                                           |
| AC-8  | When the User clicks button “No”<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields                                                                                                                                                                                          |
| AC-9  | There are 3 types of service categories:<br><br>- analyses<br>- consultation<br>- diagnostics                                                                                                                                                                                                                                        |

## Fields description

|       |                  |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| ----- | ---------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**   | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-1   | Service name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:**<br><br>- Given the name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the name”                                                                                                                                                                                                                                                                                           |
| F-2   | Price            | **Required:** yes<br><br>**Type:** number input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the field contains non-numeric symbolsOr entered price is below or equals 0And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid price”<br>- Given the phone number field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the price”                                           |
| F-3   | Service category | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User selects a service category fieldThen the System displays the list of categories in the dropdown<br>- When the User selects the service category from dropdownThen the System should fill the service category field with this service category <br>- Given the service category field is empty And the field loses focusThen the border of the field becomes redAnd an error message of a missing value is shown to the User “Please, choose the service category” |
| F-4   | Status           | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Default value:** Active<br><br>**Values:** “Active”, “Inactive”                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |

### US-42 Change service’s status

_As a RECEPTIONIST_
_I want to change service’s status_
_so as I can mark service that is not available anymore_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the modal window/page for creation of specialization, or on “Specialization information” page, or on the modal window/page for creation of service, or on “Service information” page

#### Acceptance criteria

|       |                                                                                                                                                                  |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                  |
| AC-1  | When the User changes the status of the service<br><br>Then the system changes status of this service in the database                                            |
| AC-2  | When the User changes status to “Inactive”<br><br>Then this service becomes invisible for patients to choose to make an appointment or to look information about |

## Fields description

|       |                |                                                                                         |
| ----- | -------------- | --------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                         |
| F-1   | Status         | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive” |

### US-43 Edit service

_As a RECEPTIONIST_
_I want to edit service information
\_so as I can correct some mistakes in it_

## Background

## Prototype

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Service information” page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                        |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                        |
| AC-1  | When the User clicks on “Edit” button<br><br>Then all the fields on the profile page become editable                                                                                                                                                                   |
| AC-2  | 2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                          |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the “Save changes” button is disabled                                                                                                          |
| AC-4  | Given all of the required fields are completed<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the information of this Service in the system<br><br>And the page to view service information is displayed with updated data |
| AC-5  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                               |
| AC-6  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                           |
| AC-7  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved                                                                                                        |
| AC-8  | When the User clicks “No” button <br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields                                                                                                       |

#### Fields description

|       |                  |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| ----- | ---------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**   | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-1   | Service name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:**<br><br>- Given the name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the name”                                                                                                                                                                                                                                                                                           |
| F-2   | Price            | **Required:** yes<br><br>**Type:** number input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the field contains non-numeric symbolsOr entered price is below or equals 0And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid price”<br>- Given the phone number field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the price”                                           |
| F-3   | Service category | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User selects a service category fieldThen the System displays the list of categories in the dropdown<br>- When the User selects the service category from dropdownThen the System should fill the service category field with this service category <br>- Given the service category field is empty And the field loses focusThen the border of the field becomes redAnd an error message of a missing value is shown to the User “Please, choose the service category” |
| F-4   | Status           | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |

### US-44 View service’s info

_As a RECEPTIONIST_
_I want to view service’s info_
_so as I can view all service’s information_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Specialization’s info” page

#### Acceptance criteria

|       |                                                                                                                                                                                              |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                              |
| AC-1  | When the Receptionist clicks on the row of the service<br><br>Then the service page with the whole information is displayed                                                                  |
| AC-2  | Service’s information page consists of the fields with the following information below:<br><br>- service name<br>- price<br>- service category<br>- status - radiobuttons (Active, Inactive) |
| AC-3  | The page should contain “Edit” button to edit information                                                                                                                                    |

# Task Implement Appointments API

### US-6 Create an appointment

_As a PATIENT_
_I want to create an appointment_
_so as I can solve a health problem_

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                  |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                  |
| AC-1  | The button “Make an appointment” must be available to press from every page                                                                                                                                                                                                      |
| AC-2  | When a User clicks the button “Make an appointment”<br>Then the modal window must be displayed                                                                                                                                                                                   |
| AC-3  | The modal window must contain the following fields:<br>- specialization - combobox<br>- doctor - combobox<br>- service - combobox<br>- office - dropdown<br>- date - datepicker<br>- time slots - table with slots<br>- button “Confirm”<br>- "x" button in the top right corner |
| AC-4  | Given a User isn’t signed in<br>When the User clicks button “Confirm”<br>Then a notification “Sign in to make an appointment”<br>And the modal window to sign in is displayed                                                                                                    |
| AC-5  | Given a User is signed in<br>And all the fields are completed<br>When a User clicks button “Confirm”<br>Then a notification “Appointment has been created” is displayed<br>And appointment is added in database                                                                  |
| AC-6  | Given at least one of the fields is empty<br>Or at least one of the fields is invalid<br>Then the button “Confirm” is disabled                                                                                                                                                   |
| AC-7  | When a User clicks "x" button in the top right corner<br>Then a dialog window “Do you really want to exit? Your appointment will not be saved.” is displayed                                                                                                                     |
| AC-8  | The dialog window must contain the following fields:<br>- “Yes” button<br>- “No” button                                                                                                                                                                                          |
| AC-9  | When the User clicks button “Yes”<br>Then the dialog window has to be closed<br>And the modal window of creation has to be closed                                                                                                                                                |
| AC-10 | When the User clicks button “No”<br>Then the dialog window has to be closed<br>And the modal window of creation has to be displayed with already entered fields                                                                                                                  |
| AC-11 | Given Specialization and Service fields are completed<br>Then Date and Time Slots fields get enabled                                                                                                                                                                             |
| AC-12 | List of doctors contains only doctors with “At work” status                                                                                                                                                                                                                      |
| AC-13 | List of offices contains only offices with “Active” status                                                                                                                                                                                                                       |
| AC-14 | List of specializations contains only specializations with “Active” status                                                                                                                                                                                                       |
| AC-15 | List of services contains only services with “Active” status                                                                                                                                                                                                                     |

#### Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| ----- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-1   | Specialization | **Required:** yes<br>**Type:** combobox<br>**Default value:** empty<br>**Behaviour:** <br>- When the User starts typing in a specialization title Then the System displays the filtered list of specializations in the dropdown<br>- When the User selects the specialization from drop-down listThen the System should fill the specialization field with this specialization<br>- Given entered specialization name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid specialization name”<br>- Given doctor field is completedThen specialization field has to be filled with doctor’s specialization<br>- Given service field is completedThen specialization field has to be filled according to service’s specialization<br>- Given office field is completedThen drop-down list must contain specializations according to the specializations of doctors from the office <br>- Given the specialization field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the specialization”                                                                                                                                                                                                                                                   |
| F-2   | Doctor         | **Required:** yes<br>**Type:** combobox<br>**Default value:** empty<br>**Behaviour:** <br>- When the User starts typing in the doctor's name Or the User clicks on the doctor fieldThen the System displays the filtered result (if available) of doctors in the dropdown<br>- When the User selects the doctor from drop-down listThen the System should fill the doctor field with this doctor<br>- Given entered doctor name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid doctor name”<br>- Given specialization field is completedThen drop-down list must contain doctors according to the specialization<br>- Given service field is completedThen drop-down list must contain doctors according to the services<br>- Given office field is completedThen drop-down list must contain doctors according to the the office <br>- Given the doctor field is emptyAnd time and date are completedThen drop-down list must contain doctors according to the selected time and date<br>- Given the doctor field is emptyAnd time and date are completedAnd only 1 doctor is free at this timeThen doctor field is completed with free doctor<br>- Given the doctor field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the doctor” |
| F-3   | Service        | **Required:** yes<br>**Type:** combobox<br>**Default value:** empty<br>**Behaviour:** <br>- When a User enters service nameThen the drop-down list of services according to entered name is displayed<br>- When the User selects the service from drop-down listThen the System should fill the service field with this service<br>- Given entered service name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid service name”<br>- Given specialization field is completedThen drop-down list must contain services according to the specialization<br>- Given doctor field is completedThen drop-down list must contain services according to the doctor’s specialization<br>- Given office field is completedThen drop-down list must contain services according to the specializations of doctors from the office <br>- Given the service field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the service”                                                                                                                                                                                                                                                                                                                          |
| F-4   | Office         | **Required:** yes<br>**Type:** dropdown<br>**Default value:** empty<br>**Behaviour:** <br>- When the User selects an office fieldThen the System displays the list of offices in the dropdown<br>- When the User selects the office from dropdownThen the System should fill the office field with this office<br>- Given specialization field is completedThen drop-down list must contain offices that have doctors with such a specialization<br>- Given doctor field is completedThen office field has to be filled with doctor’s office address<br>- Given service field is completedThen drop-down list must contain offices that have doctors with such a specialization<br>- Given the office field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, choose the office”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-5   | Date           | See US “Select Date and Time”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| F-6   | Timeslots      | See US “Select Date and Time”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |

### US-7 Select Date and Time Slot

### US-7 Select Date and Time Slot

_As a PATIENT_
_I want to select time and date of appointment_
_so as I can solve a health problem at the appropriate time_

## Preconditions

- User is on the “Make an appointment” modal window
- Specialisation and service fields are completed

## Acceptance criteria

|       |                                                                                                                                                                                                                  |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                  |
| AC-1  | Time slots are represented in increments of 10 minutes                                                                                                                                                           |
| AC-2  | The quantity of time slots is reserved according to the service                                                                                                                                                  |
| AC-3  | Given the chosen service belongs to consultation<br><br>And the time slot in increments of 10 minutes is free<br><br>Then the time slot is displayed                                                             |
| AC-4  | Given the chosen service belongs to diagnostics<br><br>And the time slot in increments of 10 minutes is free<br><br>And the time slot in increments of 20 minutes is free<br><br>Then the time slot is displayed |
| AC-5  | Given the chosen service belongs to analyses<br><br>Then 1 time slot of 10 minutes is reserved for 1 appointment                                                                                                 |
| AC-6  | Given the chosen service belongs to consultation<br><br>Then 2 time slots of 10 minutes are reserved for 1 appointment                                                                                           |
| AC-7  | Given the chosen service belongs to diagnostics<br><br>Then 3 time slots of 10 minutes are reserved for 1 appointment                                                                                            |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ----- | -------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| F-5   | Date           | **Required:** yes<br><br>**Type:** datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given there are no free time slots on the dateThen the date is unable to pick<br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, select the date”                                                                                                                                              |
| F-6   | Timeslots      | **Required:** yes<br><br>**Type:** table of slots<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date is selectedThen free time slots due to the date, the specialisation, the service, the doctor (optional) are displayed<br>- Given the date is not selectedThen time slots are disabled to view<br>- Given the time slot field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, select the time slot” |

### US-10 View appointment schedule by doctor

_As a DOCTOR_
_I want to view my appointment schedule_
_so as I can view my work schedule and understand timetable of the day_

#### Preconditions

- Doctor is signed in

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                         |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                         |
| AC-1  | When a Doctor clicks on “My schedule”<br>Then a page with the table of appointments on the current day is displayed                                                                                                                                     |
| AC-2  | The table should contain following fields<br>- time for appointment (Ex. 11:00 - 11:20 if initial consultation)<br>- full name of the patient - link<br>- service name<br>- approvement status<br>- link to add/view medical results of the appointment |
| AC-3  | The table should be ordered ascending by time                                                                                                                                                                                                           |
| AC-4  | Given the Receptionist approved an appointment<br>Then the status of this appointment is “Approved”                                                                                                                                                     |
| AC-5  | Given the Receptionist didn’t approve an appointment<br>Then the status of this appointment is “Not approved”                                                                                                                                           |
| AC-6  | The page should contain datepicker for the Doctor to be able to choose another date                                                                                                                                                                     |
| AC-7  | Given the Doctor picked another date<br>Then the appointment schedule for this day is displayed                                                                                                                                                         |
| AC-8  | Given the status of appointment is “Approved”<br>Then full name of the patient link is active                                                                                                                                                           |
| AC-9  | When the Doctor follows the active full name of the patient link<br>Then the patient’s profile is displayed                                                                                                                                             |
| AC-10 | Given the status of appointment is “Approved”<br>Then link to add medical results of the appointment is active                                                                                                                                          |
| AC-11 | When the Doctor follows the active link to add medical results of the appointment <br>Then the page for entering medical results is displayed                                                                                                           |

### US-13 View appointment list by receptionist

_As a RECEPTIONIST_
_I want to view appointment list_
_so as I can view all the appointments of the clinic and plan the working day_

#### Preconditions

- Receptionist is signed in

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                      |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                      |
| AC-1  | When a Receptionist clicks on “Appointments”<br>Then a page with the filtration fields and button “Generate” is displayed                                                                                                                                                                            |
| Ac-2  | When the Receptionist clicks the button “Generate”<br>Then the table according to chosen filtration criteria is displayed below                                                                                                                                                                      |
| AC-3  | The table should contain the following fields<br>- appointment time (Ex. 11:00 - 11:20 - if initial consultation)<br>- full name of the doctor (first name, last name, middle name)<br>- full name of the patient (first name, last name, middle name)<br>- patient’s phone number<br>- service name |
| AC-4  | Every row of the table should contain “Approve” and “Cancel” buttons                                                                                                                                                                                                                                 |
| AC-5  | The table should be ordered ascending by time                                                                                                                                                                                                                                                        |
| AC-6  | The page should contain the datepicker for filtration by appointment date                                                                                                                                                                                                                            |
| AC-7  | Given the Receptionist picks another date<br>Then the appointment list for this day is displayed                                                                                                                                                                                                     |
| AC-8  | The page should contain the field for filtration by doctor full name                                                                                                                                                                                                                                 |
| AC-9  | The page should contain the field for filtration by service name                                                                                                                                                                                                                                     |
| AC-10 | The page should contain the field for filtration by appointment status (Approved, Not Approved, All)                                                                                                                                                                                                 |
| AC-11 | The page should contain the field for filtration by office                                                                                                                                                                                                                                           |
| AC-12 | Given the appointments’ times are equal<br>Then they should be alphabetically ordered ascending by doctor surname                                                                                                                                                                                    |
| AC-13 | Given the appointments’ times are equal<br>And the appointments’ doctor surnames are equal<br>Then they should be alphabetically ordered ascending by doctor name                                                                                                                                    |
| AC-14 | Given the appointments’ times are equal<br>And the appointments’ doctor surnames are equal<br>And the appointments’ doctor names are equal<br>Then they should be alphabetically ordered ascending by service name                                                                                   |
| AC-15 | The list can be filtered by several fields at the same time                                                                                                                                                                                                                                          |
| AC-16 | The page should contain the “Create an appointment” button to create an appointment                                                                                                                                                                                                                  |
| AC-17 | Every row should have icon/button to reschedule an appointment                                                                                                                                                                                                                                       |

### US-14 Approve appointment

_As a RECEPTIONIST_
_I want to approve appointment_
_so as the doctor is aware of  approved appointment, patients and his schedule_

#### Preconditions

- Receptionist is signed in
- Receptionist is on the page “Appointments”

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                            |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                            |
| AC-1  | When the Receptionist clicks the button “Approve”<br>Then this appointment gets status “Approved”<br>And the row of the table with this appointment gets special highlighting that clearly marks them as “approved” for the User<br>And the button “Approve” gets disabled |

### US-15 Cancel appointment

_As a RECEPTIONIST_
_I want to cancel appointment_
*so as another client can make an appointment at released time slot* 

#### Preconditions

- Receptionist is signed in
- Receptionist is on the page “Appointments”

#### Acceptance criteria

|       |                                                                                                                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                      |
| AC-1  | When the Receptionist clicks the “Cancel” button<br>Then a dialog window “Do you really want to cancel the appointment? It will be permanently deleted.” is displayed                |
| AC-2  | The dialog window must contain the following buttons:<br>- “Yes” button<br>- “No” button                                                                                             |
| AC-3  | When the Receptionist clicks “Yes” button<br>Then the dialog window is  closed<br>And the appointment is  removed from the table<br>And the appointment is deleted from the database |
| AC-4  | When the User clicks “No” button<br>Then the dialog window is  closed<br>And the table is displayed without changes                                                                  |

### US-45 View appointment history by doctor

_As a DOCTOR_
_I want to view patient’s appointment history_
_so as I can view all of the appointments of the patient and previous results_

#### Preconditions

- Doctor is signed in
- Doctor is on patient’s profile page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                       |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                       |
| AC-1  | When the Doctor clicks on “Appointment results” tab<br>Then the page with the list/table of appointments of this patient is displayed                                                                                                                                                 |
| AC-2  | The list/table should contain the following fields<br>- appointment date<br>- appointment time (Ex. 11:00 - 11:20 - if initial consultation)<br>- full name of the doctor (first name, last name, middle name)<br>- service name<br>- link to view medical results of the appointment |
| AC-3  | The table should be ordered descending by date                                                                                                                                                                                                                                        |
| AC-4  | Given the appointments have equal datesThen this appointments should be ordered ascending by time                                                                                                                                                                                     |

### US-46 View appointment history by patient

_As a PATIENT_
_I want to view my appointment history_
_so as I can view all my appointments_

#### Preconditions

- Patient is signed in
- Patient is on profile page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                       |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                       |
| AC-1  | When the Patient clicks on “Appointment results” tab<br>Then the page with the list/table of appointments of this patient is displayed                                                                                                                                                |
| AC-2  | The list/table should contain the following fields<br>- appointment date<br>- appointment time (Ex. 11:00 - 11:20 - if initial consultation)<br>- full name of the doctor (first name, last name, middle name)<br>- service name<br>- link to view medical results of the appointment |
| AC-3  | The table should be ordered descending by date                                                                                                                                                                                                                                        |
| AC-4  | Given the appointments have equal datesThen this appointments should be ordered ascending by time                                                                                                                                                                                     |
| AC-5  | Every row should have icon/button to reschedule an appointment                                                                                                                                                                                                                        |

### US-58 Create appointment result

_As a DOCTOR_
_I want to create appointment result_
_so as I can describe current health stage of the patient and give some recommendations_

#### Preconditions

- Doctor is signed in
- Doctor is on the “My schedule” page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| AC-1  | When a Doctor clicks on link to add medical results<br>And this appointment doesn’t have related result<br>Then the modal window/page for result creation is displayed                                                                                                                                                                                                                                                                                                                                                |
| AC-2  | The modal window/page for the result creation must contain the following fields:<br>- date of the result<br>- full name of the patient (first name, last name, middle name)<br>- patient’s date of birth<br>- full name of the doctor (first name, last name, middle name)<br>- doctor’s specialization<br>- service name<br>- complaints - text required field<br>- conclusion - text required field<br>- recommendations - text required field<br>And 2 footer buttons:<br>- “Confirm” button <br>- “Cancel” button |
| AC-3  | Given at least one of the required fields is empty<br>Or at least one of the required fields is invalid<br>Then the button “Confirm” is disabled                                                                                                                                                                                                                                                                                                                                                                      |
| AC-4  | Given all of the required fields are completed<br>When the Doctor clicks the “Confirm” button<br>Then the system should add the Result to the system                                                                                                                                                                                                                                                                                                                                                                  |
| AC-5  | The date field, as well as the fields: full name of the patient, patient’s date of birth, full name of the doctor are prefilled with the data of related appointment                                                                                                                                                                                                                                                                                                                                                  |
| AC-6  | When the User clicks "Cancel" button<br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                                                                                                                                                                                                                                                                                                             |
| AC-7  | The dialog window must contain the following buttons:<br>- “Yes” button<br>- “No” button                                                                                                                                                                                                                                                                                                                                                                                                                              |
| AC-8  | When the User clicks “Yes” button<br>Then the dialog window has to be closed<br>And the page “My schedule” is displayed                                                                                                                                                                                                                                                                                                                                                                                               |
| AC-9  | When the User clicks button “No”<br>Then the dialog is closed<br>And the page for creation is displayed with already entered fields                                                                                                                                                                                                                                                                                                                                                                                   |

#### Fields description

|       |                 |                                                                                                                                                                                                                                                                                                          |
| ----- | --------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**  | **Description**                                                                                                                                                                                                                                                                                          |
| F-1   | Complaints      | **Required:** yes<br>**Type:** text area<br>**Default value:** empty<br>**Behaviour:** <br>- Given the complaints field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the complaints”           |
| F-2   | Conclusion      | **Required:** yes<br>**Type:** text area<br>**Default value:** empty<br>**Behaviour:** <br>- Given the conclusion field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the conclusion”           |
| F-3   | Recommendations | **Required:** yes<br>**Type:** text area<br>**Default value:** empty<br>**Behaviour:** <br>- Given the recommendations field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the recommendations” |
|       |                 |                                                                                                                                                                                                                                                                                                          |

### US-59 Edit result information

_As a DOCTOR_
_I want to edit result information_
_so as I can correct some mistakes in result_

#### Preconditions

- Doctoris signed in
- Doctor is on the result information page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                 |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                 |
| AC-1  | When the User clicks on “Edit” button<br>Then the fields complaints, diagnosis? conclusion, recommendations on the result information page become editable                                                                                                      |
| AC-2  | 2 footer buttons are visible in Edit mode<br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                       |
| AC-3  | Given at least one of the required fields is empty<br>Or at least one of the required fields is invalid<br>Then the “Save changes” button is disabled                                                                                                           |
| AC-4  | Given all of the required fields are completed<br>When the User clicks the “Save changes” button<br>Then the system should update the result information of this Result in the system<br>And the page to view result information is displayed with updated data |
| AC-5  | When the User clicks "Cancel" button<br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                            |
| AC-6  | The dialog window must contain the following buttons:<br>- “Yes” button<br>- “No” button                                                                                                                                                                        |
| AC-7  | When the User clicks “Yes” button<br>Then the dialog window has to be closed<br>And the page switches to View mode<br>And the changes are not saved                                                                                                             |
| AC-8  | When the User clicks button “No”<br>Then the dialog window has to be closed<br>And the page for editing has to be displayed with already entered fields                                                                                                         |

#### Fields description

|       |                 |                                                                                                                                                                                                                                                                                                          |
| ----- | --------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**  | **Description**                                                                                                                                                                                                                                                                                          |
| F-1   | Complaints      | **Required:** yes<br>**Type:** text area<br>**Default value:** empty<br>**Behaviour:** <br>- Given the complaints field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the complaints”           |
| F-2   | Conclusion      | **Required:** yes<br>**Type:** text area<br>**Default value:** empty<br>**Behaviour:** <br>- Given the conclusion field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the conclusion”           |
| F-3   | Recommendations | **Required:** yes<br>**Type:** text area<br>**Default value:** empty<br>**Behaviour:** <br>- Given the recommendations field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the recommendations” |

### US-60 View appointment result by doctor

_As a DOCTOR_
_I want to view appointment result_
_so as I can view the correctness of the result_

#### Preconditions

- Doctor is signed in
- Doctor is on the “My schedule” page or on patient’s “Appointment results” page

#### Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                      |
| AC-1  | When a Doctor clicks on link to add/view medical results<br>And this appointment has related result<br>Then the page with result information is displayed                                                                                                                                                                                                                            |
| AC-2  | Result information page consists of the fields with the following information below:<br>- date of the result<br>- full name of the patient (first name, last name, middle name)<br>- patient’s date of birth<br>- full name of the doctor (first name, last name, middle name)<br>- doctor’s specialization<br>- service name<br>- complaints <br>- conclusion <br>- recommendations |
| AC-3  | Given a Doctor who looks through the result information is the doctor linked to this appointment<br>Then “Edit” button to edit information is visible                                                                                                                                                                                                                                |

### US-61 View appointment result by patient

_As a PATIENT_
_I want to view appointment result_
_so as I can view results of my appointment and understand the stage of my health_

#### Preconditions

- Patient is signed in
- Patient is on profile page on “Appointment results” tab

#### Acceptance criteria

| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                                   |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1  | When a Patient clicks on link to view medical results<br>Then the page with result information is displayed                                                                                                                                                                                                                                                                                       |
| AC-2  | Result information page consists of the fields with the following information below:<br>- date of the result<br>- full name of the patient (first name, last name, middle name)<br>- patient’s date of birth<br>- full name of the doctor (first name, last name, middle name)<br>- doctor’s specialization<br>- service name<br>- complaints<br>- conclusion<br>- Diagnisis<br>- recommendations |
| AC-3  | The page should contain button to download result                                                                                                                                                                                                                                                                                                                                                 |

### US-62 Download appointment result

_As a PATIENT_
_I want to download appointment result_
_so as I can save my results on my device_

#### Preconditions

- Patient is signed in
- Patient is on view appointment result page

#### Acceptance criteria

|       |                                                                                            |
| ----- | ------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                            |
| AC-1  | When the User presses Download button <br>Then the result is downloaded to the user device |
| AC-2  | Files are able to download in the following formats:<br>- pdf<br>**TBD**                   |
| AC-3  | Data should be filled in the table and converted to some file format                       |

# Implement Profiles API

### US-4 View doctors

_As a PATIENT_
_I want to view doctors_
_so as I can have a look at  all the specialists of clinic and after that choose appropriate one_

## Acceptance criteria

|       |                                                                                                                                                                                 |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                 |
| AC-1  | When a User clicks on text/icon to move to the doctors page<br><br>Then the page with the filtration fields and list of the doctor cards is displayed                           |
| AC-2  | Only doctors with “At work” status are displayed                                                                                                                                |
| AC-3  | The doctor card should contain the following fields:<br><br>- photo<br>- full name (first name, last name, middle name)<br>- specialization<br>- experience<br>- office address |
| AC-4  | Experience should be calculated as:<br><br>current year - career year start + 1                                                                                                 |
| AC-5  | The page should contain the field for search by doctor full name                                                                                                                |
| AC-6  | The page should contain the field for filtration by specialization                                                                                                              |
| AC-7  | The page should contain the field for filtration by office                                                                                                                      |
| AC-8  | The page should contain the button/icon for filtering by office and viewing offices on map                                                                                      |
| AC-9  | The list can be filtered by several fields at the same time                                                                                                                     |

### US-8 Create patient’s profile

_As a PATIENT_
_I want to create profile_
_so as I can create personal profile and after that make an appointment as an authorised user_

## Preconditions:

- User followed the link after Sign up

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                                                                                          |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                                          |
| AC-1  | When the User follows the link in email<br><br>Then the page for the profile creation is displayed<br><br>And account of this user gets “true” value in “isEmailVerified” field                                                                                                                                                                                                                          |
| AC-2  | The page for the profile creation must contain the following fields:<br><br>- photo - attached file<br>- first name - text required field<br>- last name - text required field<br>- middle name - text field<br>- phone number - number text field<br>- date of birth - datepicker<br>- button “Confirm”                                                                                                 |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled                                                                                                                                                                                                                                                 |
| AC-4  | Given all of the required fields are completed<br><br>When the User clicks the “Confirm” button<br><br>Then the system should try to find matches among the profiles whose “isLinkedToAccount” field value is false                                                                                                                                                                                      |
| AC-5  | Given no matches are found<br><br>Then the profile with entered data is created                                                                                                                                                                                                                                                                                                                          |
| AC-6  | Given a match with profile has been found in the system<br><br>And “isLinkedToAccount” field value of this profile is false<br><br>Then the window with the message “A similar profile has been found, you might have already visited one of our clinics?” and 2 footer buttons: “Yes, it’s me”, “No, it’s not me” – is displayed<br><br>And the profile information of an existing account is displayed |
| AC-7  | When the User clicks “Yes, it’s me” button<br><br>Then the existing profile is linked to the account of this user<br><br>And “isLinkedToAccount” field of this profile gets true value                                                                                                                                                                                                                   |
| AC-8  | When the User clicks “No, it’s not me” button<br><br>Or no matches with other accounts are found<br><br>Then new profile with entered information is created in the system<br><br>And new profile is linked to the account of this user<br><br>And “isLinkedToAccount” field of this profile gets true value                                                                                             |
| AC-9  | **Rules of match:**<br><br>Every field in profile has weight coefficient:<br><br>- first name - 5<br>- last name - 5<br>- middle name - 5<br>- date of birth - 3<br><br>If the sum of weight coefficient of fields that match with the fields of existing profile >= 13, then the existing profile is considered as matched and is displayed to User.                                                    |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-1   | Photo          | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-2   | First name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name”                                                                                                                                                                                                                                                                              |
| F-3   | Last name      | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”                                                                                                                                                                                                                                                                                |
| F-4   | Middle name    | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| F-5   | Phone number   | **Required:** yes<br><br>**Type:** number input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Phone number field contains + prefix that cannot be deleted<br>- Given the field contains non-numeric symbolsAnd the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid phone number”<br>- Given the phone number field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the phone number” |
| F-6   | Date of birth  | **Required:** yes<br><br>**Type:** datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, select the date”                                                                                                                                                                                                                                                                                           |

### US-9 Create doctor’s profile

_As a RECEPTIONIST_
_I want to create profile for doctor_
_so as doctor can automate some of his work_

## Preconditions

- Receptionist is signed in
- Receptionist is on page “Doctors”

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| AC-1  | When the Receptionist clicks “Create doctor” button <br><br>Then the modal window/page for creation is displayed                                                                                                                                                                                                                                                                                                                                                                                        |
| AC-2  | The modal window/page for the doctor’s profile creation must contain the following fields:<br><br>- photo - attached file<br>- first name - text required field<br>- last name - text required field<br>- middle name - text field<br>- date of birth - datepicker<br>- e-mail - email text required field<br>- specialization - combobox<br>- office - dropdown<br>- career start year- datepicker<br>- status - dropdown<br><br>And 2 footer buttons:<br><br>- “Confirm” button <br>- “Cancel” button |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled                                                                                                                                                                                                                                                                                                                                                |
| AC-4  | Given all of the required fields are completed<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Doctor to the system<br><br>And send an email with doctor’s credentials to the entered email address                                                                                                                                                                                                                                                      |
| AC-5  | A password for doctor should be generated by the system                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| AC-6  | Date fields are less or equal to current date                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| AC-7  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                                                                                                                                                                                                                                                                                           |
| AC-8  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                                                                                                                                                                                                                                            |
| AC-9  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page “Doctors” is displayed                                                                                                                                                                                                                                                                                                                                                                             |
| AC-10 | When the User clicks button “No”<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields                                                                                                                                                                                                                                                                                                                                                             |

## Fields description

|       |                   |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ----- | ----------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**    | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| F-1   | Photo             | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| F-2   | First name        | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name”                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| F-3   | Last name         | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| F-4   | Middle name       | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| F-5   | Date of birth     | **Required:** yes<br><br>**Type:** datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And the error message of a missing value is shown to a User “Please, select the date”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| F-6   | E-mail            | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email exist in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email already exists”                                                                |
| F-7   | Specialization    | **Required:** yes<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in a specialization title Then the System displays the filtered list of specialisation in the dropdown<br>- When the User selects the specialization from dropdownThen the System should fill the specialization field with this specialization <br>- Given the entered specialization name doesn’t existAnd the field loses focusThen an error message of an invalid value is shown to a User “Invalid specialization name”<br>- Given the specialization field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the specialisation” |
| F-8   | Office            | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User selects an office fieldThen the System displays the list of offices in the dropdown<br>- When the User selects the office from dropdownThen the System should fill the office field with this office<br>- Given the office field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, choose the office”                                                                                                                                                                                                                                                     |
| F-9   | Career start year | **Required:** yes<br><br>**Type:** year datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, select the year”                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| F-10  | Status            | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** At work<br><br>**Behaviour:** <br><br>- When the User selects a status fieldThen the System displays the list of statuses (“At work”, “On vacation”, “Sick Day”, “Sick Leave”, “Self-isolation”, “Leave without pay”, “Inactive”) in the dropdown<br>- When the User selects the status from dropdownThen the System should fill the status field with this status                                                                                                                                                                                                                                                                                                                                           |

### US-11 View patient’s profile by patient

_As a PATIENT_
_I want to view my profile_
_so as I can view my personal information_

## Preconditions

- Patient is signed in

## Acceptance criteria

|       |                                                                                                                                                                                       |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                       |
| AC-1  | When the Patient clicks on profile icon<br><br>Then the profile page is displayed                                                                                                     |
| AC-2  | The profile page must consist of 2 tabs:<br><br>1. Personal information<br>2. Appointment results                                                                                     |
| AC-3  | Personal information tab opens by default                                                                                                                                             |
| AC-4  | Personal information tab contains fields with the following information below:<br><br>- photo <br>- first name <br>- last name <br>- middle name<br>- phone number<br>- date of birth |

### US-12 View patient’s profile by doctor

_As a DOCTOR_
_I want to view patient’s profile_
_so as I can find out some basic information about the patient_

## Preconditions

- Doctor is signed in
- Doctor is on the page “My schedule”

## Acceptance criteria

|       |                                                                                                                                                                                       |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                       |
| AC-1  | Given the appointment has status “Approved”<br><br>When the Doctor follows the active full name of the patient link<br><br>Then the profile page of the patient is displayed          |
| AC-2  | Given the status of appointment isn’t “Approved”<br><br>Then full name of the patient link cannot be followed                                                                         |
| AC-3  | The profile page must consist of 2 tabs:<br><br>1. Personal information<br>2. Appointment results                                                                                     |
| AC-4  | Personal information tab opens by default                                                                                                                                             |
| AC-5  | Personal information tab contains fields with the following information below:<br><br>- photo <br>- first name <br>- last name <br>- middle name<br>- phone number<br>- date of birth |

### US-16 View doctor information

_As a PATIENT_
_I want to view doctor information_
*so as I can view more detailed information about doctor* 

## Preconditions

- User in on “Doctors“ page

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                      |
| AC-1  | When a User clicks on the doctor’s card<br><br>Then the page with personal information of the doctor is displayed                                                                                                                                                                                                                    |
| AC-2  | The doctor page should contain the following fields:<br><br>- photo<br>- full name (first name, last name, middle name)<br>- office address<br>- experience<br>- specialization<br>- list of services according to specialization<br>- “Make an appointment with the doctor” button<br>- button/icon to go back to view doctors list |
| AC-3  | Experience should be calculated as:<br><br>current year - career year start + 1                                                                                                                                                                                                                                                      |
| AC-4  | When the Patient clicks “Make an appointment with the doctor” button<br><br>Then the modal window to create an appointment is displayed<br><br>And the doctor field is completed with this doctor (whose information page is opened)                                                                                                 |

### US-17 View doctor’s profile by doctor

_As a DOCTOR_
_I want to view my profile_
_so as I can view my personal information_

## Preconditions

- Doctor is signed in

## Acceptance criteria

|       |                                                                                                                                                                                                                                 |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                 |
| AC-1  | When the Doctor clicks on profile icon<br><br>Then the profile page with the personal information is displayed                                                                                                                  |
| AC-2  | Personal information page consists of the fields with the following information below:<br><br>- photo<br>- first name<br>- last name<br>- middle name<br>- date of birth<br>- specialization<br>- office<br>- career start year |
| AC-3  | The page should contain “Edit” button to edit information                                                                                                                                                                       |

### US-18 Edit doctor’s profile

_As a DOCTOR, RECEPTIONIST_
_I want to edit profile_
_so as I can correct some mistakes in personal information_

## Preconditions

- Doctor, Receptionist is signed in
- Doctor, Receptionist is on the “Profile page”

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                   |
| ----- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                   |
| AC-1  | When the User clicks on “Edit” button<br><br>Then all the fields on the profile page become editable                                                                                                                                                              |
| AC-2  | 2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                     |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the “Save changes” button is disabled                                                                                                     |
| AC-4  | Given all of the required fields are completed<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the profile information of this Doctor in the system<br><br>And the page to view profile is displayed with updated data |
| AC-5  | Date fields are less or equal to current date                                                                                                                                                                                                                     |
| AC-6  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                          |
| AC-7  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                      |
| AC-8  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved                                                                                                   |
| AC-9  | When the User clicks button “No”<br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields                                                                                                   |

## Fields description

|       |                   |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| ----- | ----------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name**    | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| F-1   | Photo             | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| F-2   | First name        | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name”                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| F-3   | Last name         | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-4   | Middle name       | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| F-5   | Date of birth     | **Required:** yes<br><br>**Type:** datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And the error message of a missing value is shown to the User “Please, select the date”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| F-6   | Specialization    | **Required:** yes<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in a specialization title Then the System displays the filtered list of specialisation in the combobox<br>- When the User selects the specialization from drop-down listThen the System should fill the specialization field with this specialization<br>- Given the entered specialization name doesn’t existAnd the field loses focusThen an error message of an invalid value is shown to a User “Invalid specialization name”<br>- Given the specialization field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the specialisation” |
| F-7   | Office            | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in an office addressThen the System displays the filtered list of offices in the dropdown<br>- When the User selects the office from drop-down listThen the System should fill the office field with this office <br>- Given the office field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, choose the office”                                                                                                                                                                                                                               |
| F-8   | Career start year | **Required:** yes<br><br>**Type:** year datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, select the year”                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| F-9   | Status            | **Required:** yes<br><br>**Type:** dropdown<br><br>**Behaviour:** <br><br>- When the User selects a status fieldThen the System displays the list of statuses (“At work”, “On vacation”, “Sick Day”, “Sick Leave”, “Self-isolation”, “Leave without pay”, “Inactive”) in the dropdown<br>- When the User selects the status from dropdownThen the System should fill the status field with this status                                                                                                                                                                                                                                                                                                                                                                                  |

### US-19 Filter doctor list by specialization

_As a PATIENT_
_I want to filter doctor list by specialization_
_so as I can faster find the appropriate doctor_

## Preconditions

- User is on the “Doctors” page

## Acceptance criteria

|       |                                                                                                                                                            |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                            |
| AC-1  | The “Doctors” page contains field for filtration by specialization                                                                                         |
| AC-2  | When the User selects the specialization from the combobox<br><br>Then the System displays doctor cards which specialization matches with the selected one |
| AC-3  | Given no doctors match the selected filtration<br><br>Then the page should display text “There are no doctors matching this filtration”                    |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ----- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| F-1   | Specialization | **Required:** no<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in a specialization title Then the System displays the filtered list of specializations in the combobox<br>- When the User selects the specialization from drop-down listThen the System should fill the specialization field with this specialization<br>- Given office field is completedThen drop-down list must contain specializations according to the specializations of doctors from the office <br>- Given entered specialization name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Incorrect specialization” |

### US-20 Change doctor’s status

_As a RECEPTIONIST_
_I want to change doctor’s status_
_so as I can mark doctor that is not working or vacationing_

## Preconditions

- Receptionist is signed in
- Receptionist is on “Doctors” page

## Acceptance criteria

|       |                                                                                                                     |
| ----- | ------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                     |
| AC-1  | When the User changes the status of the doctor<br><br>Then the system changes status of this doctor in the database |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                        |
| ----- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                        |
| F-1   | Status         | **Required:** yes<br><br>**Type:** dropdown<br><br>**Behaviour:** <br><br>- When the User selects a status fieldThen the System displays the list of statuses (“At work”, “On vacation”, “Sick Day”, “Sick Leave”, “Self-isolation”, “Leave without pay”, “Inactive”) in the dropdown<br>- When the User selects the status from dropdownThen the System should fill the status field with this status |

### US-21 Filter doctor list by office

_As a PATIENT_
_I want to filter doctor list by office_
_so as I can faster find the appropriate doctor in the appropriate office_

## Preconditions

- User is on the “Doctors” page

## Acceptance criteria

|       |                                                                                                                                                                                                                |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                |
| AC-1  | The “Doctors” page contains field for filtration by office                                                                                                                                                     |
| AC-2  | When the User selects the office from the dropdown<br><br>Then the System displays doctor cards which office matches with the selected one<br><br>And the System should fill the office field with this office |
| AC-3  | Given no doctors match the selected filtration<br><br>Then the page should display text “There are no doctors matching this filtration”                                                                        |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| ----- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| F-1   | Office         | **Required:** no<br><br>**Type:** dropdown<br><br>**Default value:** All<br><br>**Behaviour:** <br><br>- When the User clicks the office fieldThen the System displays the list of offices in the dropdown<br>- When the User selects an office from dropdownThen the System should fill the office field with this office<br>- Given specialization field is completedThen drop-down list must contain offices that have doctors with the given specialization |

### US-22 Filter doctor list by office by admin

_As a RECEPTIONIST_
_I want to filter doctor list by office_
_so as I can faster find the appropriate doctor in the appropriate office_

## Preconditions

- Receptionist is signed in
- Receptionist is on “Doctors“ page

## Acceptance criteria

|       |                                                                                                                                                                                                               |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                               |
| AC-1  | The “Doctors” page contains field for filtration by office                                                                                                                                                    |
| AC-2  | When the User selects the office from the dropdown<br><br>Then the System displays doctor rows which office matches with the selected one<br><br>And the System should fill the office field with this office |
| AC-3  | Given no doctors match the selected filtration<br><br>Then the page should display text “There are no doctors matching this filtration”                                                                       |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| ----- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| F-1   | Office         | **Required:** no<br><br>**Type:** dropdown<br><br>**Default value:** All<br><br>**Behaviour:** <br><br>- When the User clicks the office fieldThen the System displays the list of offices in the dropdown<br>- When the User selects an office from dropdownThen the System should fill the office field with this office<br>- Given specialization field is completedThen drop-down list must contain offices that have doctors with the given specialization |

### US-23 Filter doctor list by office on map

_As a PATIENT_
_I want to filter doctor list by office on map_
_so as I can faster find the appropriate doctor in the appropriate office_

## Preconditions

- User is on the “Doctors” page

## Acceptance criteria

|       |                                                                                                                                                 |
| ----- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                 |
| AC-1  | The “Doctors” page contains button/icon to view offices on map                                                                                  |
| AC-2  | When the User clicks the button/icon <br><br>Then the System displays a map with some marks of the offices on it                                |
| AC-3  | When the User clicks on the mark of the office on the map<br><br>Then the modal window with information about the office is displayed           |
| AC-4  | The modal window must contain the following attributes<br><br>- photo<br>- address<br>- button “Select”<br>- "x" button in the top right corner |
| AC-5  | When the User clicks "x" button in the top right corner<br><br>Then the modal window should be closed                                           |
| AC-6  | When User clicks "Select" button<br><br>Then the office is selected as a field for filtration in the office field                               |

### US-24 Filter doctor list by specialization by admin

_As a RECEPTIONIST_
_I want to filter doctor list by specialization_
_so as I can faster find the appropriate doctor_

## Preconditions

- Receptionist is signed in
- Receptionist is on “Doctors“ page

## Acceptance criteria

|       |                                                                                                                                                           |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                           |
| AC-1  | The “Doctors” page contains field for filtration by specialization                                                                                        |
| AC-2  | When the User selects the specialization from the combobox<br><br>Then the System displays doctor rows which specialization matches with the selected one |
| AC-3  | Given no doctors match the selected filtration<br><br>Then the page should display “There are no doctors matching this filtration”                        |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ----- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| F-1   | Specialization | **Required:** no<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in a specialization title Then the System displays the filtered list of specializations in the combobox<br>- When the User selects the specialization from drop-down listThen the System should fill the specialization field with this specialization<br>- Given office field is completedThen drop-down list must contain specializations according to the specializations of doctors from the office <br>- Given entered specialization name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Incorrect specialization” |

### US-25 Search doctor by name

_As a PATIENT_
_I want to search doctor by name_
_so as I can faster find the appropriate doctor_

## Preconditions

- User is on the “Doctors” page

## Acceptance criteria

|       |                                                                                                       |
| ----- | ----------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                       |
| AC-1  | The “Doctors” page contains field for search by doctor full name (first name, last name, middle name) |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| ----- | -------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| F-1   | Doctor         | **Required:** no<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in the doctor's name Then the System displays the filtered result (if available) of doctors in the dropdown<br>- When the User selects the doctor from drop-down listThen the System should fill the doctor field with this doctor full nameAnd display the card of this doctor<br>- Given entered doctor name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid doctor name” |

### US-26 Search doctor by name by admin

_As a RECEPTIONIST_
_I want to search doctor by name_
_so as I can faster find the appropriate doctor_

## Preconditions

- User is on the “Doctors” page

## Acceptance criteria

|       |                                                                                                       |
| ----- | ----------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                       |
| AC-1  | The “Doctors” page contains field for search by doctor full name (first name, last name, middle name) |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| ----- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| F-1   | Doctor         | **Required:** no<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in the doctor's name Then the System displays the filtered result (if available) of doctors in the dropdown<br>- When the User selects the doctor from drop-down listThen the System should fill the doctor field with this doctor full nameAnd display the row of this doctor in table<br>- Given entered doctor name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid doctor name” |

### US-27 View doctor’s profile by admin

_As a RECEPTIONIST_
_I want to view doctor’s profile_
_so as I can view doctor’s personal information_

## Preconditions

- Receptionist is signed in
- Receptionist is on the “Doctors” page

## Acceptance criteria

|       |                                                                                                                                                                                                                                             |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                             |
| AC-1  | When the Receptionist clicks on the row of the doctor<br><br>Then the profile page with the personal information is displayed                                                                                                               |
| AC-2  | Personal information page consists of the fields with the following information below:<br><br>- photo<br>- first name<br>- last name<br>- middle name<br>- date of birth<br>- specialization<br>- office<br>- career start year<br>- status |
| AC-3  | The page should contain “Edit” button to edit information                                                                                                                                                                                   |

### US-28 View doctors by admin

_As a RECEPTIONIST_
_I want to view doctors_
_so as I can have a look at all the specialists of clinic_

## Preconditions

- Receptionist is signed in

## Acceptance criteria

|       |                                                                                                                                                                                         |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                         |
| AC-1  | When the Receptionist clicks on “Doctors” menu item<br><br>Then the page with the table of the doctors is displayed                                                                     |
| AC-2  | The table should contain the following fields<br><br>- full name (first name, last name, middle name)<br>- specialization<br>- status - dropdown<br>- date of birth<br>- office address |
| AC-3  | The page should contain the field for search by doctor full name                                                                                                                        |
| AC-4  | The page should contain the field for filtration by specialization                                                                                                                      |
| AC-5  | The page should contain the field for filtration by office                                                                                                                              |
| AC-6  | The list can be filtered by several fields at the same time                                                                                                                             |
| AC-7  | The page should contain the “Create doctor” button to create doctor’s profile                                                                                                           |

### US-47 Create patient’s profile by admin

_As a RECEPTIONIST_
*I want to create patient’s profile* 
_so as I can create profile for patient who wants to make an appointment offline and attach results later_

## Preconditions:

- Receptionist is signed in
- Receptionist is on page “Patients” or on modal window to create an appointment

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                       |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                                                                       |
| AC-1  | When the Receptionist clicks “Create patient” button <br><br>Then the modal window/page for creation is displayed                                                                                                                                                                                                     |
| AC-2  | The modal window/page for the patient’s profile creation must contain the following fields:<br><br>- first name - text required field<br>- last name - text required field<br>- middle name - text field<br>- date of birth - datepicker<br><br>And 2 footer buttons:<br><br>- “Confirm” button <br>- “Cancel” button |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled                                                                                                                                                              |
| AC-4  | Given all of the required fields are completed<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Patient to the system                                                                                                                                                   |
| AC-5  | Date fields are less or equal to current date                                                                                                                                                                                                                                                                         |
| AC-6  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                                                                                                         |
| AC-7  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                                                          |
| AC-8  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page “Patients” is displayed                                                                                                                                                                                          |
| AC-9  | When the User clicks button “No”<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields                                                                                                                                                                           |
| AC-10 | By default,  a created patient’s profile has  ‘false’ value in “isLinkedToAccount” field                                                                                                                                                                                                                              |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                 |
| ----- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                 |
| F-1   | First name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name” |
| F-2   | Last name      | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”   |
| F-3   | Middle name    | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                    |
| F-4   | Date of birth  | **Required:** yes<br><br>**Type:** datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, select the date”              |

### US-48 Delete patient’s profile

_As a RECEPTIONIST_
_I want to delete patient’s profile_
_so as I can remove all the personal data of the patient if he wants_

## Preconditions

- Receptionist is signed in
- Receptionist is on “Patients” page

## Acceptance criteria

|       |                                                                                                                                                                                                                           |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                           |
| AC-1  | When the User clicks on “Delete” button<br><br>Then a dialog window “Do you really want to delete? Data will be lost.” is displayed                                                                                       |
| AC-2  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                              |
| AC-3  | When the User clicks “Yes” button <br><br>Then the dialog window is closed<br><br>And the row of this patient is removed from table<br><br>And the patient’s profile and all the related data are removed from the system |
| AC-4  | When the User clicks “No” button<br><br>Then the dialog window is closed                                                                                                                                                  |

### US-49 Edit patient’s profile

_As a PATIENT, RECEPTIONIST_
_I want to edit profile_
_so as I can correct some mistakes in personal information_

## Preconditions

- Patient, Receptionist is signed in
- Patient, Receptionist is on the “Profile page” on the “Personal information” tab

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                    |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                                    |
| AC-1  | When the User clicks on “Edit” button<br><br>Then all the fields on the profile page become editable                                                                                                                                                               |
| AC-2  | 2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                      |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the “Save changes” button is disabled                                                                                                      |
| AC-4  | Given all of the required fields are completed<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the profile information of this Patient in the system<br><br>And the page to view profile is displayed with updated data |
| AC-5  | Date fields are less or equal to current date                                                                                                                                                                                                                      |
| AC-6  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                           |
| AC-7  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                       |
| AC-8  | When the User clicks “Yes” button<br><br>Then the dialog window is  closed<br><br>And the page switches to View mode<br><br>And the changes are not saved                                                                                                          |
| AC-9  | When the User clicks button “No”<br><br>Then the dialog window is closed<br><br>And the page for editing is displayed with already entered fields                                                                                                                  |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-1   | Photo          | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-2   | First name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name”                                                                                                                                                                                                                                                                              |
| F-3   | Last name      | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”                                                                                                                                                                                                                                                                                |
| F-4   | Middle name    | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| F-5   | Phone number   | **Required:** yes<br><br>**Type:** number input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Phone number field contains + prefix that cannot be deleted<br>- Given the field contains non-numeric symbolsAnd the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid phone number”<br>- Given the phone number field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the phone number” |
| F-6   | Date of birth  | **Required:** yes<br><br>**Type:** datepicker<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the date field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, select the date”                                                                                                                                                                                                                                                                                         |

### US-50 Search patient by name by admin

_As a RECEPTIONIST_
_I want to search patient by name_
_so as I can faster find the patient I’m looking for_

## Preconditions

- Receptionist is on the “Patients” page

## Acceptance criteria

|       |                                                                                                         |
| ----- | ------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                         |
| AC-1  | The “Patients” page contains field for search by patient full name (first name, last name, middle name) |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ----- | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| F-1   | Patient        | **Required:** no<br><br>**Type:** combobox<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in the patient's name Then the System displays the filtered result (if available) of patients in the dropdown<br>- When the User selects the patient from drop-down listThen the System should fill the patient field with this patient full nameAnd displays the row of this patient in the table<br>- Given the entered patient name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “No matches found” |

### US-51 View patient’s profile by admin

_As a RECEPTIONIST_
_I want to view patient’s profile_
_so as I can view patient’s personal information_

## Preconditions

- Receptionist is signed in

## Acceptance criteria

|       |                                                                                                                                                                                                                                                        |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                        |
| AC-1  | Given the Receptionist is on the “Patients” page <br><br>When the Receptionist clicks on the row of the patient<br><br>Or clicks on the patient row in the combobox for search<br><br>Then the profile page with the personal information is displayed |
| AC-2  | Given the Receptionist is on the “Appointments” page <br><br>When the Receptionist clicks on the patient’s full name link<br><br>Then the profile page with the personal information is displayed                                                      |
| AC-3  | Personal information tab contains fields with the following information below:<br><br>- photo <br>- first name <br>- last name <br>- middle name<br>- phone number<br>- date of birth                                                                  |
| AC-4  | “Appointment results” tab is invisible for admin  (See US View patient’s profile by doctor)                                                                                                                                                            |
| AC-5  | The page should contain “Edit” button to edit information                                                                                                                                                                                              |

### US-52 View patients by admin

_As a RECEPTIONIST_
_I want to view patients_
_so as I can have a look at the list of all the patients of the clinic_

## Preconditions

- Receptionist is signed in

## Acceptance criteria

|       |                                                                                                                         |
| ----- | ----------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                         |
| AC-1  | When the Receptionist clicks on “Patients” menu item<br><br>Then the page with the table of the patients is displayed   |
| AC-2  | The table should contain the following fields<br><br>- full name (first name, last name, middle name)<br>- phone number |
| AC-3  | The page should contain the field for search by patient full name                                                       |
| AC-4  | The page should contain the “Create patient” button to create patient’s profile                                         |
| AC-5  | Every patient row should contain “Delete” button                                                                        |

### US-53 Create receptionist’s profile

_As a RECEPTIONIST_
_I want to create profile for another receptionist_
_so as new worker can automate some of his work_

## Preconditions

- Receptionist is signed in
- Receptionist is on page “Receptionists”

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                                                                                                                                |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                                                                                                                                                |
| AC-1  | When the Receptionist clicks “Create receptionist” button <br><br>Then the modal window/page for creation is displayed                                                                                                                                                                                                                                                         |
| AC-2  | The modal window/page for the doctor’s profile creation must contain the following fields:<br><br>- photo - attached file<br>- first name - text required field<br>- last name - text required field<br>- middle name - text field<br>- e-mail - email text required field<br>- office - dropdown<br><br>and 2 footer buttons:<br><br>- “Confirm” button <br>- “Cancel” button |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled                                                                                                                                                                                                                       |
| AC-4  | Given all of the required fields are completed<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Receptionist to the system<br><br>And send an email with receptionist’s credentials to the entered email address                                                                                                                 |
| AC-5  | A password for receptionist should be generated by the system                                                                                                                                                                                                                                                                                                                  |
| AC-6  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed                                                                                                                                                                                                                                  |
| AC-8  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                                                                                                                                   |
| AC-9  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page “Receptionists” is displayed                                                                                                                                                                                                                                              |
| AC-10 | When the User clicks button “No”<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields                                                                                                                                                                                                                                    |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ----- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| F-1   | Photo          | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| F-2   | First name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name”                                                                                                                                                                                                                                                                                                                                                                                                     |
| F-3   | Last name      | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”                                                                                                                                                                                                                                                                                                                                                                                                       |
| F-4   | Middle name    | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| F-5   | E-mail         | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email exist in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email already exists” |
| F-6   | Office         | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User selects an office fieldThen the System displays the list of offices in the dropdown<br>- When the User selects the office from dropdownThen the System should fill the office field with this office<br>- Given the office field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, choose the office”                                                                                                                                                                                      |

### US-54 Delete receptionist’s profile

_As a RECEPTIONIST_
_I want to delete receptionist_
_so as I can remove receptionist that is not working in our clinic anymore_

## Preconditions

- Receptionist is signed in
- Receptionist is on “Receptionists” page

## Acceptance criteria

|       |                                                                                                                                                                                                                                         |
| ----- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                         |
| AC-1  | When the User clicks on “Delete” button<br><br>Then a dialog window “Do you really want to delete? Data will be lost.” is displayed                                                                                                     |
| AC-2  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                            |
| AC-3  | When the User clicks “Yes” button <br><br>Then the dialog window has to be closed<br><br>And the row of this receptionist is removed from table<br><br>And the profile, account, photo of this receptionist are removed from the system |
| AC-4  | When the User clicks “No” button<br><br>Then the dialog window has to be closed                                                                                                                                                         |

### US-55 Edit receptionist’s profile

_As a RECEPTIONIST_
_I want to edit profile_
_so as I can correct some mistakes in personal information_

## Preconditions

- Receptionist is signed in
- Receptionist is on the “Profile page” or on the page to view receptionist’s information

## Acceptance criteria

|       |                                                                                                                                                                                                                                                                         |
| ----- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                                                                                         |
| AC-1  | When the User clicks on “Edit” button<br><br>Then all the fields on the profile page become editable                                                                                                                                                                    |
| AC-2  | 2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button                                                                                                                                                                           |
| AC-3  | Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the “Save changes” button is disabled                                                                                                           |
| AC-4  | Given all of the required fields are completed<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the profile information of this Receptionist in the system<br><br>And the page to view profile is displayed with updated data |
| AC-5  | Date fields are less or equal to current date                                                                                                                                                                                                                           |
| AC-6  | When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed                                                                                                                                |
| AC-7  | The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button                                                                                                                                                                            |
| AC-8  | When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved                                                                                                         |
| AC-9  | When the User clicks button “No”<br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields                                                                                                         |

## Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| ----- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-1   | Photo          | **Required:** no<br><br>**Type:** file-uploader<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-2   | First name     | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the first name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the first name”                                                                                                                                                                                                                                           |
| F-3   | Last name      | **Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- Given the last name field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the last name”                                                                                                                                                                                                                                             |
| F-4   | Middle name    | **Required:** no<br><br>**Type:** text input<br><br>**Default value:** empty                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| F-5   | Office         | **Required:** yes<br><br>**Type:** dropdown<br><br>**Default value:** empty<br><br>**Behaviour:** <br><br>- When the User starts typing in an office addressThen the System displays the filtered list of offices in the dropdown<br>- When the User selects the office from drop-down listThen the System should fill the office field with this office <br>- Given the office field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, choose the office” |

### US-56 View receptionist’s profile

_As a RECEPTIONIST_
_I want to view receptionist’s profile_
_so as I can view receptionist’s personal information_

## Preconditions

- Receptionist is signed in

## Acceptance criteria

|       |                                                                                                                                                                                                 |
| ----- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                                                                                 |
| AC-1  | When the Receptionist clicks on the row of the receptionist<br><br>And the Receptionist is on the “Receptionists” page <br><br>Then the profile page with the personal information is displayed |
| AC-2  | When the Receptionist clicks on profile icon<br><br>Then the profile page with the personal information is displayed                                                                            |
| AC-3  | Personal information page consists of the fields with the following information below:<br><br>- photo<br>- first name<br>- last name<br>- middle name<br>- office                               |
| AC-4  | The page should contain “Edit” button to edit information                                                                                                                                       |

### US-57 View receptionists

_As a RECEPTIONIST_
_I want to view receptionists_
_so as I can have a look at all the receptionists of clinic_

## Preconditions

- Receptionist is signed in

## Acceptance criteria

|       |                                                                                                                                 |
| ----- | ------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Description**                                                                                                                 |
| AC-1  | When the Receptionist clicks on “Receptionists” menu item<br><br>Then the page with the table of the receptionists is displayed |
| AC-2  | The table should contain the following fields<br><br>- full name (first name, last name, middle name)<br>- office address       |
| AC-3  | Every receptionist row should contain “Delete” button                                                                           |
| AC-4  | The page should contain the “Create receptionist” button to create receptionist’s profile                                       |
