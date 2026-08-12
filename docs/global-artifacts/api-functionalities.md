# API Documentation

## Table of Contents

1. [AuthController](#authcontroller)
2. [OperationRequestController](#operationrequestcontroller)
3. [OperationTypeController](#operationtypecontroller)
4. [PatientController](#patientcontroller)
5. [StaffController](#staffcontroller)
6. [UserController](#usercontroller)

---

### AuthController

Handles user authentication and account management functionalities.

- **`Login`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/auth/login`
    - **Description**: Authenticates a user and returns a JWT token.
    - **Parameters**: `LoginDto` in the request body.
    - **Responses**:
        - `200 OK`: Successfully authenticated with token.
        - `400 Bad Request` or `500 Internal Server Error`: Error during login process.

- **`ActivateAccount`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/auth/activate-account`
    - **Description**: Activates a user account based on provided password details.
    - **Parameters**: `PasswordDto` in the request body.
    - **Responses**:
        - `200 OK`: Account activated successfully.
        - `400 Bad Request` or `500 Internal Server Error`: Error during activation.

- **`ResetPasswordRequest`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/auth/reset-password`
    - **Description**: Requests a password reset for a user.
    - **Parameters**: `ResetPasswordRequestDTO` in the request body.
    - **Responses**:
        - `200 OK`: Request processed successfully.
        - `500 Internal Server Error`: Error during request.

- **`ChangePassword`**
    - **HTTP Method**: `PUT`
    - **Route**: `/api/auth/change-password`
    - **Description**: Changes the user’s password.
    - **Parameters**: `PasswordDto` in the request body.
    - **Responses**:
        - `200 OK`: Password changed successfully.
        - `500 Internal Server Error`: Error during password change.

---

### OperationRequestController

Manages operation requests and associated functions.

- **`Create`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/operationrequest`
    - **Description**: Creates a new operation request.
    - **Parameters**: `CreateOperationRequestDto` in the request body.
    - **Responses**:
        - `201 Created`: Operation request successfully created.
        - `400 Bad Request`: Invalid request data.

- **`GetById`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/operationrequest/{id}`
    - **Description**: Retrieves an operation request by its ID.
    - **Responses**:
        - `200 OK`: Operation request found and returned.
        - `404 Not Found`: Operation request not found.

- **`GetAll`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/operationrequest`
    - **Description**: Retrieves all operation requests.
    - **Responses**:
        - `200 OK`: Operation requests retrieved successfully.
        - `400 Bad Request`: Error retrieving operation requests.

- **`Update`**
    - **HTTP Method**: `PUT`
    - **Route**: `/api/operationrequest/{id}`
    - **Description**: Updates an existing operation request.
    - **Parameters**: `OperationRequestDto` in the request body.
    - **Responses**:
        - `200 OK`: Operation request updated successfully.
        - `400 Bad Request`: Error during update.

- **`Delete`**
    - **HTTP Method**: `DELETE`
    - **Route**: `/api/operationrequest/{id}`
    - **Description**: Deletes an operation request.
    - **Responses**:
        - `204 No Content`: Operation request successfully deleted.
        - `400 Bad Request`: Error during deletion.

- **`Search`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/operationrequest/search`
    - **Description**: Searches for operation requests based on provided criteria.
    - **Parameters**: `OperationRequestCriteria` in the query parameters.
    - **Responses**:
        - `200 OK`: Search results returned successfully.
        - `400 Bad Request`: Error during search.

---

### OperationTypeController

Handles management of operation types.

- **`GetAllOperationTypesAsync`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/operationtype`
    - **Description**: Retrieves all available operation types.
    - **Responses**:
        - `200 OK`: Operation types retrieved successfully.
        - `404 Not Found`: No operation types found.

- **`GetAllByStatus`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/operationtype?status={status}`
    - **Description**: Retrieves operation types filtered by status.
    - **Parameters**: Status (`int`) as a query parameter.
    - **Responses**:
        - `200 OK`: Operation types found and returned.
        - `500 Internal Server Error`: Error retrieving operation types.

---

### PatientController

Handles operations related to patient management.

- **`CreatePatient`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/patient`
    - **Description**: Creates a new patient record.
    - **Parameters**: `CreatePatientDTO` in the request body.
    - **Responses**:
        - `201 Created`: Patient record created successfully.
        - `500 Internal Server Error`: Error during creation.

- **`ListPatientsByFilter`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/patient/search`
    - **Description**: Lists patients based on search criteria.
    - **Parameters**: `PatientCriteria` as query parameters.
    - **Responses**:
        - `200 OK`: Patients found and returned.
        - `404 Not Found`: No patients matching the criteria.

- **`UpdatePatient`**
    - **HTTP Method**: `PUT`
    - **Route**: `/api/patient/{medicalRecordNumber}`
    - **Description**: Updates patient details.
    - **Parameters**: `PatientCriteria` in the request body.
    - **Responses**:
        - `200 OK`: Patient details updated successfully.
        - `404 Not Found`: Patient not found.

- **`ConfirmPatientDelete`**
    - **HTTP Method**: `DELETE`
    - **Route**: `/api/patient/confirmation/{medicalRecordNumber}`
    - **Description**: Confirms deletion of a patient record.
    - **Parameters**: Confirmation as `bool` in the query.
    - **Responses**:
        - `200 OK`: Patient deletion confirmed.
        - `400 Bad Request`: Error confirming deletion.

- **`DeletePatient`**
    - **HTTP Method**: `DELETE`
    - **Route**: `/api/patient/delete/{medicalRecordNumber}`
    - **Description**: Deletes a patient record, requiring prior confirmation.
    - **Responses**:
        - `202 Accepted`: Patient record deleted.
        - `404 Not Found`: Patient not found.

---

### StaffController

Handles operations related to staff management.

- **`CreateStaff`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/staff`
    - **Description**: Creates a new staff record.
    - **Parameters**: `CreateStaffDto` in the request body.
    - **Responses**:
        - `201 Created`: Staff record created successfully.
        - `500 Internal Server Error`: Error during creation.

- **`SearchStaffByCriteria`**
    - **HTTP Method**: `GET`
    - **Route**: `/api/staff/search`
    - **Description**: Searches for staff members based on specified criteria.
    - **Parameters**: `StaffCriteria` as query parameters.
    - **Responses**:
        - `200 OK`: Staff members found and returned.
        - `404 Not Found`: No staff members matching the criteria.

- **`UpdateStaff`**
    - **HTTP Method**: `PUT`
    - **Route**: `/api/staff/{licenseNumber}`
    - **Description**: Updates details for an existing staff member.
    - **Parameters**: `StaffCriteria` in the request body.
    - **Responses**:
        - `200 OK`: Staff details updated successfully.
        - `404 Not Found`: Staff member not found.

- **`ConfirmStaffDelete`**
    - **HTTP Method**: `DELETE`
    - **Route**: `/api/staff/{licenseNumber}`
    - **Description**: Marks a staff member for deletion.
    - **Parameters**: Confirmation (`bool`) as a query parameter.
    - **Responses**:
        - `200 OK`: Staff deletion confirmed.
        - `404 Not Found`: Staff member not found.

- **`DeleteStaff`**
    - **HTTP Method**: `DELETE`
    - **Route**: `/api/staff/delete/{licenseNumber}`
    - **Description**: Deletes a staff record, requiring prior confirmation.
    - **Responses**:
        - `202 Accepted`: Staff record deleted.
        - `404 Not Found`: Staff member not found.

---

### UserController

Handles operations related to system user management.

- **`CreateUser`**
    - **HTTP Method**: `POST`
    - **Route**: `/api/user`
    - **Description**: Creates a new system user.
    - **Parameters**: `SystemUserRequestDto` in the request body.
    - **Responses**:
        - `201 Created`: User created successfully.
        - `500 Internal Server Error`: Error during user creation.

- **`DeleteUser`**
    - **HTTP Method**: `DELETE`
    - **Route**: `/api/user/{username}`
    - **Description**: Deletes a user by username.
    - **Responses**:
        - `200 OK`: User deleted successfully.
        - `404 Not Found`: User not found.

---
