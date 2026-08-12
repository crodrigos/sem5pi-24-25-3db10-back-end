# General Project Workflow

## 1. Sprint Planning
At the beginning of each sprint, the group should meet to:
- a. Review the backlog of user stories and tasks.
- b. Discuss priorities and dependencies.
- c. Define the sprint goal and commit to the user stories the team believes it can complete within the sprint.
- d. Break down each user story into specific tasks and assign responsibilities to each student.

## 2. Daily Standups/Weekly Meetings
Each group should hold brief meetings (either daily or weekly) to ensure everyone is on track and aware of dependencies between modules.

## 3. Code Reviews
Regular peer code reviews should be held to maintain code quality and catch issues early.

## 4. Continuous Integration
Utilize a CI/CD pipeline for frequent testing and deployment to ensure that new features don’t break existing functionality.

## 5. Customer Feedback
After each sprint, present progress to the "customer" (the professor) for feedback, ensuring that any adjustments are made before moving forward.

## 6. Sprint Retrospective
At the end of each sprint, the group should hold a retrospective meeting to:
- a. Reflect on what went well, what could have gone better, and any challenges encountered during the sprint.
- b. Identify areas for improvement (both in the project workflow and technical implementation).
- c. Adjust workflows, tools, or processes for the next sprint to enhance productivity and collaboration.

## 7. Documentation
Each sprint should end with updated documentation, including a summary of progress, changes, and testing results.

# Sprint 1

## Backoffice Module

### User Stories

#### 5.1.1 Register New Backoffice Users
**As an Admin**, I want to register new backoffice users (e.g., doctors, nurses, technicians, admins) via an out-of-band process, so that they can access the backoffice system with appropriate permissions.

**Acceptance Criteria:**
- Backoffice users (e.g., doctors, nurses, technicians) are registered by an Admin via an internal process, not via self-registration.
- Admin assigns roles (e.g., Doctor, Nurse, Technician) during the registration process.
- Registered users receive a one-time setup link via email to set their password and activate their account.
- The system enforces strong password requirements for security.
- A confirmation email is sent to verify the user’s registration.

---

#### 5.1.2 Password Reset
**As a Backoffice User** (Admin, Doctor, Nurse, Technician), I want to reset my password if I forget it, so that I can regain access to the system securely.

**Acceptance Criteria:**
- Backoffice users can request a password reset by providing their email.
- The system sends a password reset link via email.
- The reset link expires after a predefined period (e.g., 24 hours) for security.
- Users must provide a new password that meets the system’s password complexity rules.

---

#### 5.1.3 Patient Registration
**As a Patient**, I want to register for the healthcare application, so that I can create a user profile and book appointments online.

**Acceptance Criteria:**
- Patients can self-register using the external IAM system.
- During registration, patients provide personal details (e.g., name, email, phone) and create a profile.
- The system validates the email address by sending a verification email with a confirmation link.
- Patients cannot list their appointments without completing the registration process.

---

#### 5.1.4 Update User Profile
**As a Patient**, I want to update my user profile, so that I can change my personal details and preferences.

**Acceptance Criteria:**
- Patients can log in and update their profile details (e.g., name, contact information, preferences).
- Changes to sensitive data, such as email, trigger an additional verification step (e.g., confirmation email).
- All profile updates are securely stored in the system.
- The system logs all changes made to the patient's profile for audit purposes.

---

#### 5.1.5 Account Deletion
**As a Patient**, I want to delete my account and all associated data, so that I can exercise my right to be forgotten as per GDPR.

**Acceptance Criteria:**
- Patients can request to delete their account through the profile settings.
- The system sends a confirmation email to the patient before proceeding with account deletion.
- Upon confirmation, all personal data is permanently deleted from the system within the legally required time frame (e.g., 30 days).
- Patients are notified once the deletion is complete, and the system logs the action for GDPR compliance.
- Some anonymized data may be retained for legal or research purposes, but all identifiable information is erased.

---

#### 5.1.6 User Login
**As a (non-authenticated) Backoffice User**, I want to log in to the system using my credentials, so that I can access the backoffice features according to my assigned role.

**Acceptance Criteria:**
- Backoffice users log in using their username and password.
- Role-based access control ensures that users only have access to features appropriate to their role (e.g., doctors can manage appointments, admins can manage users and settings).
- After five failed login attempts, the user account is temporarily locked, and a notification is sent to the admin.
- Login sessions expire after a period of inactivity to ensure security.

---

#### 5.1.7 Patient IAM Login
**As a Patient**, I want to log in to the healthcare system using my external IAM credentials, so that I can access my appointments, medical records, and other features securely.

**Acceptance Criteria:**
- Patients log in via an external Identity and Access Management (IAM) provider (e.g., Google, Facebook, or hospital SSO).
- After successful authentication via the IAM, patients are redirected to the healthcare system with a valid session.
- Patients have access to their appointment history, medical records, and other features relevant to their profile.
- Sessions expire after a defined period of inactivity, requiring reauthentication.

---

#### 5.1.8 Create Patient Profile
**As an Admin**, I want to create a new patient profile, so that I can register their personal details and medical history.

**Acceptance Criteria:**
- Admins can input patient details such as first name, last name, date of birth, contact information, and medical history.
- A unique patient ID (Medical Record Number) is generated upon profile creation.
- The system validates that the patient’s email and phone number are unique.
- The profile is stored securely in the system, and access is governed by role-based permissions.

---

#### 5.1.9 Edit Patient Profile
**As an Admin**, I want to edit an existing patient profile, so that I can update their information when needed.

**Acceptance Criteria:**
- Admins can search for and select a patient profile to edit.
- Editable fields include name, contact information, medical history, and allergies.
- Changes to sensitive data (e.g., contact information) trigger an email notification to the patient.
- The system logs all profile changes for auditing purposes.

---

#### 5.1.10 Delete Patient Profile
**As an Admin**, I want to delete a patient profile, so that I can remove patients who are no longer under care.

**Acceptance Criteria:**
- Admins can search for a patient profile and mark it for deletion.
- Before deletion, the system prompts the admin to confirm the action.
- Once deleted, all patient data is permanently removed from the system within a predefined time frame.
- The system logs the deletion for audit and GDPR compliance purposes.

---

#### 5.1.11 List/Search Patient Profiles
**As an Admin**, I want to list/search patient profiles by different attributes, so that I can view the details, edit, and remove patient profiles.

**Acceptance Criteria:**
- Admins can search patient profiles by various attributes, including name, email, date of birth, or medical record number.
- The system displays search results in a list view with key patient information (name, email, date of birth).
- Admins can select a profile from the list to view, edit, or delete the patient record.
- The search results are paginated, and filters are available to refine the search results.

---

#### 5.1.12 Create Staff Profile
**As an Admin**, I want to create a new staff profile, so that I can add them to the hospital’s roster.

**Acceptance Criteria:**
- Admins can input staff details such as first name, last name, contact information, and specialization.
- A unique staff ID (License Number) is generated upon profile creation.
- The system ensures that the staff’s email and phone number are unique.
- The profile is stored securely, and access is based on role-based permissions.

---

#### 5.1.13 Edit Staff Profile
**As an Admin**, I want to edit a staff’s profile, so that I can update their information.

**Acceptance Criteria:**
- Admins can search for and select a staff profile to edit.
- Editable fields include contact information, availability slots, and specialization.
- The system logs all profile changes, and any changes to contact information trigger a confirmation email to the staff member.
- The edited data is updated in real-time across the system.

---

#### 5.1.14 Deactivate Staff Profile
**As an Admin**, I want to deactivate a staff profile, so that I can remove them from the hospital’s active roster without losing their historical data.

**Acceptance Criteria:**
- Admins can search for and select a staff profile to deactivate.
- Deactivating a staff profile removes them from the active roster, but their historical data (e.g., appointments) remains accessible.
- The system confirms deactivation and records the action for audit purposes.

---

#### 5.1.15 List/Search Staff Profiles
**As an Admin**, I want to list/search staff profiles, so that I can see the details, edit, and remove staff profiles.

**Acceptance Criteria:**
- Admins can search staff profiles by attributes such as name, email, or specialization.
- The system displays search results in a list view with key staff information (name, email, specialization).
- Admins can select a profile from the list to view, edit, or deactivate.
- The search results are paginated, and filters are available for refining the search results.

---

#### 5.1.16 Request an Operation
**As a Doctor**, I want to request an operation, so that the Patient has access to the necessary healthcare.

**Acceptance Criteria:**
- Doctors can create an operation request by selecting the patient, operation type, priority, and suggested deadline.
- The system validates that the operation type matches the doctor’s specialization.
- The operation request includes:
    - Patient ID
    - Doctor ID
    - Operation Type
    - Deadline
    - Priority
- The system confirms successful submission of the operation request and logs the request in the patient’s medical history.

---

#### 5.1.17 Update Operation Requisition
**As a Doctor**, I want to update an operation requisition, so that the Patient has access to the necessary healthcare.

**Acceptance Criteria:**
- Doctors can update operation requests they created (e.g., change the deadline or priority).
- The system checks that only the requesting doctor can update the operation request.
- The system logs all updates to the operation request (e.g., changes to priority or deadline).
- Updated requests are reflected immediately in the system and notify the Planning Module of any changes.

---

#### 5.1.18 Remove Operation Requisition
**As a Doctor**, I want to remove an operation requisition, so that the healthcare activities are provided as necessary.

**Acceptance Criteria:**
- Doctors can delete operation requests they created if the operation has not yet been scheduled.
- A confirmation prompt is displayed before deletion.
- Once deleted, the operation request is removed from the patient’s medical record and cannot be recovered.
- The system notifies the Planning Module and updates any schedules that were relying on this request.

---

#### 5.1.19 List/Search Operation Requisitions
**As a Doctor**, I want to list/search operation requisitions, so that I can see the details, edit, and remove operation requisitions.

**Acceptance Criteria:**
- Doctors can search operation requests by patient name, operation type, priority, and status.
- The system displays a list of operation requests in a searchable and filterable view.
- Each entry in the list includes operation request details (e.g., patient name, operation type, status).
- Doctors can select an operation request to view, update, or delete it.

---

#### 5.1.20 Add New Operation Types
**As an Admin**, I want to add new types of operations, so that I can reflect the available medical procedures in the system.

**Acceptance Criteria:**
- Admins can add new operation types with attributes like:
    - Operation Name
    - Required Staff by Specialization
    - Estimated Duration
- The system validates that the operation name is unique.
- The system logs the creation of new operation types and makes them available for scheduling immediately.

---

#### 5.1.21 Edit Existing Operation Types
**As an Admin**, I want to edit existing operation types, so that I can update or correct information about the procedure.

**Acceptance Criteria:**
- Admins can search for and select an existing operation type to edit.
- Editable fields include operation name, required staff by specialization, and estimated duration.
- Changes are reflected in the system immediately for future operation requests.
- Historical data is maintained, but new operation requests will use the updated operation type information.

---

#### 5.1.22 Remove Obsolete Operation Types
**As an Admin**, I want to remove obsolete or no longer performed operation types, so that the system stays current with hospital practices.

**Acceptance Criteria:**
- Admins can search for and mark operation types as inactive (rather than deleting them) to preserve historical records.
- Inactive operation types are no longer available for future scheduling but remain in historical data.
- A confirmation prompt is shown before deactivating an operation type.

---

#### 5.1.23 List/Search Operation Types
**As an Admin**, I want to list/search operation types, so that I can see the details, edit, and remove operation types.

**Acceptance Criteria:**
- Admins can search and filter operation types by name, specialization, or status (active/inactive).
- The system displays operation types in a searchable list with attributes such as name, required staff, and estimated duration.
- Admins can select an operation type to view, edit, or deactivate it.
