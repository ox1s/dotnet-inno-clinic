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

|   |   |   |
|---|---|---|
|**#**|**Field name**|**Description**|
|F-1|E-mail|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email exists in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email already exists”|
|F-2|Password|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, enter the password”|
|F-3|Re-entered password|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Label:** Repeat entered password<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, reenter the password”<br>- When the re-entered password doesn’t coincide with the password And the field loses focusThen the border of the field becomes red And the error message is shown to the User “The passwords you’ve entered don’t coincide”|
### US-2 Sign in

_As a PATIENT_

_I want to sign in_

_so as I can make an appointment with doctor and check info in personal page_

#### Preconditions

- User isn’t signed in

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User clicks button/icon to sign in <br><br>OR the User clicks button “Confirm an appointment” <br><br>Then a modal window must be displayed|
|AC-2|The modal window must contain the following attributes<br><br>- e-mail - email text required field<br>- password - password text required field<br>- button “Sign in”<br>- link “Sign up”<br>- "x" button in the top right corner|
|AC-3|When the User clicks the “Sign in” button <br><br>Then the system must check if this account exists in the system|
|AC-4|Given the account exists in the system <br><br>Then the system should display a notification "You've signed in successfully"|
|AC-5|Given the account doesn’t exist in the system <br><br>Then the system should display a notification “Either an email or a password is incorrect”|
|AC-6|When a User clicks the “Sign up” link <br><br>Then the modal window to sign up must be displayed|
|AC-7|When User clicks "x" button in the top right corner<br><br>Then the modal window should be closed|
|AC-8|Given at least one of the fields is empty<br><br>Or at least one of the fields is invalid<br><br>Then the button “Sign in” is disabled|

#### Fields description

|   |   |   |
|---|---|---|
|**#**|**Field name**|**Description**|
|F-1|E-mail|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email doesn’t exist in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email doesn’t exist”|
|F-2|Password|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, enter the password”|

### US-3 Sign out

_As a PATIENT_

_I want to sign out_

_so as I can change user or end work with system_

#### Preconditions

- User is signed in

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When a User clicks “Sign out” button Then user token must be deleted|

### US-34 Sign in as a worker

_As a DOCTOR, RECEPTIONIST_

_I want to sign in_

_so as I can do my work using the system_

#### Preconditions

- User isn’t signed in
- User opened the system/application

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User opens the systemThen the modal window to sign in is displayed|
|AC-2|The modal window must contain the following attributes<br><br>- e-mail - email text required field<br>- password - password text required field<br>- button “Sign in”|
|AC-3|When the User clicks the “Sign in” button <br><br>Then the system must check if this account exists in the system|
|AC-4|Given the account exists in the system   <br>And the profile linked to the account has status “At work” or “On vacation”, “Sick Day”, “Sick Leave”, “Self-isolation”, “Leave without pay”<br><br>Then the system should display a notification "You've signed in successfully"<br><br>And display the home page|
|AC-5|Given the account doesn’t exist in the system <br><br>Or the profile linked to the account has status “Inactive”<br><br>Then the system should display a notification “Either an email or a password is incorrect”|
|AC-6|User can’t use the system while not signed in|

#### Fields description

|   |   |   |
|---|---|---|
|**#**|**Field name**|**Description**|
|F-1|E-mail|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** e-mail<br><br>**Behaviour:** <br><br>- Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”<br>- Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”<br>- Given the email doesn’t exist in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email doesn’t exist”|
|F-2|Password|**Required:** yes<br><br>**Type:** text input<br><br>**Default value:** empty<br><br>**Valid value:** min 6 symbols, max 15 symbols<br><br>**Behaviour:**<br><br>- Entered symbols must be hidden<br>- When a User clicks on some button/icon (eye icon) Then the entered password is shown<br>- When the password field is emptyAnd the field loses focus Then the border of the field becomes redAnd the error message is shown to the User “Please, enter the password”|
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
_so as I can view office details and edit if necessary_ 
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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User changes the status of the office<br><br>Then the system changes status of this office in the database|
|AC-2|When the User changes status to “Inactive”<br><br>Then all the doctors and receptionists related to this office get status “Inactive”|

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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the Receptionist clicks “Create specialization” button <br><br>Then the modal window/page for creation is displayed|
|AC-2|The modal window/page for the specialization creation must contain a field for entering the name of specialization, a field for status selecting, the table of services and two footer buttons: “Confirm” and “Cancel”|
|AC-3|The table of services should contain the following fields<br><br>- service name<br>- price<br>- status - radiobuttons (Active, Inactive)<br>- service’s category name|
|AC-4|Given the name of specialization is completed<br><br>And table of services  contains minimum 1 row<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Specialization to the system|
|AC-5|When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed|
|AC-6|The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button|
|AC-7|When the User clicks “Yes” button<br><br>Then the dialog window is closed<br><br>And the page “Specializations” is displayed|
|AC-8|When the User clicks “No” button<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields|
|AC-9|Table of services should contain “Add service” button to add new service (new row)|
|AC-10|Given the name of specialization is empty Or the table is empty<br><br>Then the button “Confirm” is disabled|

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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User changes the status of the specialization<br><br>Then the system changes status of this specialization in the database|
|AC-2|When the User changes status to “Inactive”<br><br>Then all the doctors and services related to this specialization get status “Inactive”|

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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User clicks on “Edit” button<br><br>Then the field for entering the name of specialization is editable<br><br>And the table of services should contain “Add service” button to add new service (new row)|
|AC-2|2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button|
|AC-3|Given the name of specialization is emptyOr table is empty<br><br>Then the “Save changes” button is disabled|
|AC-4|Given the name of specialization is completedAnd table isn’t empty<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the information of this Specialization in the system<br><br>And the page to view specialization information is displayed with updated data|
|AC-5|When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed|
|AC-6|The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button|
|AC-7|When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved|
|AC-8|When the User clicks “No” button <br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields|

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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the Receptionist clicks “Add service” button <br><br>Then the modal window/page for creation is displayed|
|AC-2|The modal window/page for the service’s creation must contain the following fields:<br><br>- service name - text required field<br>- price -  numeric text required field<br>- status - radiobuttons (Active, Inactive)<br>- service category - dropdown<br><br>and 2 footer buttons:<br><br>- “Confirm” button<br>- “Cancel” button|
|AC-3|Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the button “Confirm” is disabled|
|AC-4|Given all of the required fields are completed<br><br>When the Receptionist clicks the “Confirm” button<br><br>Then the system should add the Service to the system|
|AC-5|When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed|
|AC-6|The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button|
|AC-7|When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the previous page is displayed|
|AC-8|When the User clicks button “No”<br><br>Then the dialog is closed<br><br>And the page for creation is displayed with already entered fields|
|AC-9|There are 3 types of service categories:<br><br>- analyses<br>- consultation<br>- diagnostics|

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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User changes the status of the service<br><br>Then the system changes status of this service in the database|
|AC-2|When the User changes status to “Inactive”<br><br>Then this service becomes invisible for patients to choose to make an appointment or to look information about|

## Fields description

|       |                |                                                                                         |
| ----- | -------------- | --------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                         |
| F-1   | Status         | **Required:** yes<br><br>**Type:** radiobuttons<br><br>**Values:** “Active”, “Inactive” |

### US-43 Edit service

_As a RECEPTIONIST_
_I want to edit service information
_so as I can correct some mistakes in it_

## Background

## Prototype

#### Preconditions

- Receptionist is signed in
- Receptionist is on the “Service information” page

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User clicks on “Edit” button<br><br>Then all the fields on the profile page become editable|
|AC-2|2 footer buttons are visible in Edit mode<br><br>- “Save changes” button<br>- “Cancel” button|
|AC-3|Given at least one of the required fields is empty<br><br>Or at least one of the required fields is invalid<br><br>Then the “Save changes” button is disabled|
|AC-4|Given all of the required fields are completed<br><br>When the User clicks the “Save changes” button<br><br>Then the system should update the information of this Service in the system<br><br>And the page to view service information is displayed with updated data|
|AC-5|When the User clicks "Cancel" button<br><br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed|
|AC-6|The dialog window must contain the following buttons:<br><br>- “Yes” button<br>- “No” button|
|AC-7|When the User clicks “Yes” button<br><br>Then the dialog window has to be closed<br><br>And the page switches to View mode<br><br>And the changes are not saved|
|AC-8|When the User clicks “No” button <br><br>Then the dialog window has to be closed<br><br>And the page for editing has to be displayed with already entered fields|

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

|       |                                                                                                                                                                                                                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Description**                                                                                                                                                                                                                                                                      |
| AC-1  | The button “Make an appointment” must be available to press from every page                                                                                                                                                                                                          |
| AC-2  | When a User clicks the button “Make an appointment”<br>Then the modal window must be displayed                                                                                                                                                                                       |
| AC-3  | The modal window must contain the following fields:<br>- specialization - combobox<br>- doctor - combobox<br>- service - combobox<br>- office - dropdown<br>- date - datepicker<br>- time slots - table with slots<br>- button “Confirm”<br>- "x" button in the top right corner |
| AC-4  | Given a User isn’t signed in<br>When the User clicks button “Confirm”<br>Then a notification “Sign in to make an appointment”<br>And the modal window to sign in is displayed                                                                                                        |
| AC-5  | Given a User is signed in<br>And all the fields are completed<br>When a User clicks button “Confirm”<br>Then a notification “Appointment has been created” is displayed<br>And appointment is added in database                                                                      |
| AC-6  | Given at least one of the fields is empty<br>Or at least one of the fields is invalid<br>Then the button “Confirm” is disabled                                                                                                                                                       |
| AC-7  | When a User clicks "x" button in the top right corner<br>Then a dialog window “Do you really want to exit? Your appointment will not be saved.” is displayed                                                                                                                         |
| AC-8  | The dialog window must contain the following fields:<br>- “Yes” button<br>- “No” button                                                                                                                                                                                              |
| AC-9  | When the User clicks button “Yes”<br>Then the dialog window has to be closed<br>And the modal window of creation has to be closed                                                                                                                                                    |
| AC-10 | When the User clicks button “No”<br>Then the dialog window has to be closed<br>And the modal window of creation has to be displayed with already entered fields                                                                                                                      |
| AC-11 | Given Specialization and Service fields are completed<br>Then Date and Time Slots fields get enabled                                                                                                                                                                                 |
| AC-12 | List of doctors contains only doctors with “At work” status                                                                                                                                                                                                                          |
| AC-13 | List of offices contains only offices with “Active” status                                                                                                                                                                                                                           |
| AC-14 | List of specializations contains only specializations with “Active” status                                                                                                                                                                                                           |
| AC-15 | List of services contains only services with “Active” status                                                                                                                                                                                                                         |

#### Fields description

|       |                |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| ----- | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **#** | **Field name** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-1   | Specialization | **Required:** yes<br>**Type:** combobox<br>**Default value:** empty<br>**Behaviour:** <br>- When the User starts typing in a specialization title Then the System displays the filtered list of specializations in the dropdown<br>- When the User selects the specialization from drop-down listThen the System should fill the specialization field with this specialization<br>- Given entered specialization name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid specialization name”<br>- Given doctor field is completedThen specialization field has to be filled with doctor’s specialization<br>- Given service field is completedThen specialization field has to be filled according to service’s specialization<br>- Given office field is completedThen drop-down list must contain specializations according to the specializations of doctors from the office <br>- Given the specialization field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the specialization”                                                                                                                                                                                                                                                   |
| F-2   | Doctor         | **Required:** yes<br>**Type:** combobox<br>**Default value:** empty<br>**Behaviour:** <br>- When the User starts typing in the doctor's name Or the User clicks on the doctor fieldThen the System displays the filtered result (if available) of doctors in the dropdown<br>- When the User selects the doctor from drop-down listThen the System should fill the doctor field with this doctor<br>- Given entered doctor name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid doctor name”<br>- Given specialization field is completedThen drop-down list must contain doctors according to the specialization<br>- Given service field is completedThen drop-down list must contain doctors according to the services<br>- Given office field is completedThen drop-down list must contain doctors according to the the office <br>- Given the doctor field is emptyAnd time and date are completedThen drop-down list must contain doctors according to the selected time and date<br>- Given the doctor field is emptyAnd time and date are completedAnd only 1 doctor is free at this timeThen doctor field is completed with free doctor<br>- Given the doctor field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the doctor” |
| F-3   | Service        | **Required:** yes<br>**Type:** combobox<br>**Default value:** empty<br>**Behaviour:** <br>- When a User enters service nameThen the drop-down list of services according to entered name is displayed<br>- When the User selects the service from drop-down listThen the System should fill the service field with this service<br>- Given entered service name doesn’t existAnd the field loses focusThen an error message of a invalid value is shown to a User “Invalid service name”<br>- Given specialization field is completedThen drop-down list must contain services according to the specialization<br>- Given doctor field is completedThen drop-down list must contain services according to the doctor’s specialization<br>- Given office field is completedThen drop-down list must contain services according to the specializations of doctors from the office <br>- Given the service field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to a User “Please, choose the service”                                                                                                                                                                                                                                                                                                          |
| F-4   | Office         | **Required:** yes<br>**Type:** dropdown<br>**Default value:** empty<br>**Behaviour:** <br>- When the User selects an office fieldThen the System displays the list of offices in the dropdown<br>- When the User selects the office from dropdownThen the System should fill the office field with this office<br>- Given specialization field is completedThen drop-down list must contain offices that have doctors with such a specialization<br>- Given doctor field is completedThen office field has to be filled with doctor’s office address<br>- Given service field is completedThen drop-down list must contain offices that have doctors with such a specialization<br>- Given the office field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, choose the office”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| F-5   | Date           | See US “Select Date and Time”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| F-6   | Timeslots      | See US “Select Date and Time”                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When a Receptionist clicks on “Appointments”<br>Then a page with the filtration fields and button “Generate” is displayed|
|Ac-2|When the Receptionist clicks the button “Generate”<br>Then the table according to chosen filtration criteria is displayed below|
|AC-3|The table should contain the following fields<br>- appointment time (Ex. 11:00 - 11:20 - if initial consultation)<br>- full name of the doctor (first name, last name, middle name)<br>- full name of the patient (first name, last name, middle name)<br>- patient’s phone number<br>- service name|
|AC-4|Every row of the table should contain “Approve” and “Cancel” buttons|
|AC-5|The table should be ordered ascending by time|
|AC-6|The page should contain the datepicker for filtration by appointment date|
|AC-7|Given the Receptionist picks another date<br>Then the appointment list for this day is displayed|
|AC-8|The page should contain the field for filtration by doctor full name|
|AC-9|The page should contain the field for filtration by service name|
|AC-10|The page should contain the field for filtration by appointment status (Approved, Not Approved, All)|
|AC-11|The page should contain the field for filtration by office|
|AC-12|Given the appointments’ times are equal<br>Then they should be alphabetically ordered ascending by doctor surname|
|AC-13|Given the appointments’ times are equal<br>And the appointments’ doctor surnames are equal<br>Then they should be alphabetically ordered ascending by doctor name|
|AC-14|Given the appointments’ times are equal<br>And the appointments’ doctor surnames are equal<br>And the appointments’ doctor names are equal<br>Then they should be alphabetically ordered ascending by service name|
|AC-15|The list can be filtered by several fields at the same time|
|AC-16|The page should contain the “Create an appointment” button to create an appointment|
|AC-17|Every row should have icon/button to reschedule an appointment|
### US-14 Approve appointment

_As a RECEPTIONIST_
_I want to approve appointment_
_so as the doctor is aware of  approved appointment, patients and his schedule_
#### Preconditions

- Receptionist is signed in
- Receptionist is on the page “Appointments”

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the Receptionist clicks the button “Approve”<br>Then this appointment gets status “Approved”<br>And the row of the table with this appointment gets special highlighting that clearly marks them as “approved” for the User<br>And the button “Approve” gets disabled|
### US-15 Cancel appointment

_As a RECEPTIONIST_
_I want to cancel appointment_
_so as another client can make an appointment at released time slot_ 
#### Preconditions

- Receptionist is signed in
- Receptionist is on the page “Appointments”

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the Receptionist clicks the “Cancel” button<br>Then a dialog window “Do you really want to cancel the appointment? It will be permanently deleted.” is displayed|
|AC-2|The dialog window must contain the following buttons:<br>- “Yes” button<br>- “No” button|
|AC-3|When the Receptionist clicks “Yes” button<br>Then the dialog window is  closed<br>And the appointment is  removed from the table<br>And the appointment is deleted from the database|
|AC-4|When the User clicks “No” button<br>Then the dialog window is  closed<br>And the table is displayed without changes|
### US-45 View appointment history by doctor

_As a DOCTOR_
_I want to view patient’s appointment history_
_so as I can view all of the appointments of the patient and previous results_

#### Preconditions

- Doctor is signed in
- Doctor is on patient’s profile page

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the Doctor clicks on “Appointment results” tab<br>Then the page with the list/table of appointments of this patient is displayed|
|AC-2|The list/table should contain the following fields<br>- appointment date<br>- appointment time (Ex. 11:00 - 11:20 - if initial consultation)<br>- full name of the doctor (first name, last name, middle name)<br>- service name<br>- link to view medical results of the appointment|
|AC-3|The table should be ordered descending by date|
|AC-4|Given the appointments have equal datesThen this appointments should be ordered ascending by time|
### US-46 View appointment history by patient

_As a PATIENT_
_I want to view my appointment history_
_so as I can view all my appointments_
#### Preconditions

- Patient is signed in
- Patient is on profile page

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the Patient clicks on “Appointment results” tab<br>Then the page with the list/table of appointments of this patient is displayed|
|AC-2|The list/table should contain the following fields<br>- appointment date<br>- appointment time (Ex. 11:00 - 11:20 - if initial consultation)<br>- full name of the doctor (first name, last name, middle name)<br>- service name<br>- link to view medical results of the appointment|
|AC-3|The table should be ordered descending by date|
|AC-4|Given the appointments have equal datesThen this appointments should be ordered ascending by time|
|AC-5|Every row should have icon/button to reschedule an appointment|
### US-58 Create appointment result

_As a DOCTOR_
_I want to create appointment result_
_so as I can describe current health stage of the patient and give some recommendations_
#### Preconditions

- Doctor is signed in
- Doctor is on the “My schedule” page

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When a Doctor clicks on link to add medical results<br>And this appointment doesn’t have related result<br>Then the modal window/page for result creation is displayed|
|AC-2|The modal window/page for the result creation must contain the following fields:<br>- date of the result<br>- full name of the patient (first name, last name, middle name)<br>- patient’s date of birth<br>- full name of the doctor (first name, last name, middle name)<br>- doctor’s specialization<br>- service name<br>- complaints - text required field<br>- conclusion - text required field<br>- recommendations - text required field<br>And 2 footer buttons:<br>- “Confirm” button <br>- “Cancel” button|
|AC-3|Given at least one of the required fields is empty<br>Or at least one of the required fields is invalid<br>Then the button “Confirm” is disabled|
|AC-4|Given all of the required fields are completed<br>When the Doctor clicks the “Confirm” button<br>Then the system should add the Result to the system|
|AC-5|The date field, as well as the fields: full name of the patient, patient’s date of birth, full name of the doctor are prefilled with the data of related appointment|
|AC-6|When the User clicks "Cancel" button<br>Then a dialog window “Do you really want to cancel? Entered data will not be saved.” is displayed|
|AC-7|The dialog window must contain the following buttons:<br>- “Yes” button<br>- “No” button|
|AC-8|When the User clicks “Yes” button<br>Then the dialog window has to be closed<br>And the page “My schedule” is displayed|
|AC-9|When the User clicks button “No”<br>Then the dialog is closed<br>And the page for creation is displayed with already entered fields|

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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User clicks on “Edit” button<br>Then the fields complaints, diagnosis? conclusion, recommendations on the result information page become editable|
|AC-2|2 footer buttons are visible in Edit mode<br>- “Save changes” button<br>- “Cancel” button|
|AC-3|Given at least one of the required fields is empty<br>Or at least one of the required fields is invalid<br>Then the “Save changes” button is disabled|
|AC-4|Given all of the required fields are completed<br>When the User clicks the “Save changes” button<br>Then the system should update the result information of this Result in the system<br>And the page to view result information is displayed with updated data|
|AC-5|When the User clicks "Cancel" button<br>Then a dialog window “Do you really want to cancel? Changes will not be saved.” is displayed|
|AC-6|The dialog window must contain the following buttons:<br>- “Yes” button<br>- “No” button|
|AC-7|When the User clicks “Yes” button<br>Then the dialog window has to be closed<br>And the page switches to View mode<br>And the changes are not saved|
|AC-8|When the User clicks button “No”<br>Then the dialog window has to be closed<br>And the page for editing has to be displayed with already entered fields|

#### Fields description

|       |                 |                                                                                                                                                                                                                                                                                                                          |
| ----- | --------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **#** | **Field name**  | **Description**                                                                                                                                                                                                                                                                                                          |
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

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When a Doctor clicks on link to add/view medical results<br>And this appointment has related result<br>Then the page with result information is displayed|
|AC-2|Result information page consists of the fields with the following information below:<br>- date of the result<br>- full name of the patient (first name, last name, middle name)<br>- patient’s date of birth<br>- full name of the doctor (first name, last name, middle name)<br>- doctor’s specialization<br>- service name<br>- complaints <br>- conclusion <br>- recommendations|
|AC-3|Given a Doctor who looks through the result information is the doctor linked to this appointment<br>Then “Edit” button to edit information is visible|
### US-61 View appointment result by patient

_As a PATIENT_
_I want to view appointment result_
_so as I can view results of my appointment and understand the stage of my health_

#### Preconditions

- Patient is signed in
- Patient is on profile page on “Appointment results” tab

#### Acceptance criteria

|**#**|**Description**|
|---|---|
|AC-1|When a Patient clicks on link to view medical results<br>Then the page with result information is displayed|
|AC-2|Result information page consists of the fields with the following information below:<br>- date of the result<br>- full name of the patient (first name, last name, middle name)<br>- patient’s date of birth<br>- full name of the doctor (first name, last name, middle name)<br>- doctor’s specialization<br>- service name<br>- complaints<br>- conclusion<br>- Diagnisis<br>- recommendations|
|AC-3|The page should contain button to download result|
### US-62 Download appointment result

_As a PATIENT_
_I want to download appointment result_
_so as I can save my results on my device_

#### Preconditions

- Patient is signed in
- Patient is on view appointment result page

#### Acceptance criteria

|   |   |
|---|---|
|**#**|**Description**|
|AC-1|When the User presses Download button <br>Then the result is downloaded to the user device|
|AC-2|Files are able to download in the following formats:<br>- pdf<br>**TBD**|
|AC-3|Data should be filled in the table and converted to some file format|
