<img width="1236" height="557" alt="image" src="https://github.com/user-attachments/assets/ccef68f1-727b-4a93-9441-c2ff146755a6" /># `PraeturaLoanTask API`

## Purpose
`LoanApplicationsController` exposes HTTP API to save or query loan applications:
- `POST /api/loan-applications` - submit a new loan application
- `GET /api/loan-applications/{id}` - retrieve an application by guid `id` returned to client in `POST` call

It relies on `ILoanApplicationService` to perform validation, saving and retrieval.

`DecisionLogEntryController` - exposes HTTP API for viewing all decision log entries
- `GET /api/decision-log-entries` - retrieve all decision log entries 


## `POST` behavior
- Reads an idempotency key from the `Idempotency-Key` request header (first value, defaults to empty string).
- Calls `loanValidationService.Validate` to validate the incoming model.
  - If validation fails, `400 Bad Request` is returned with error messages.
- Calls `loanValidationService.SaveNewApplication` to save loan application.
  - If save fails `500 Internal Server Error` is returned with messages.
- On success returns `201 Created` with the saved loan application result in the response body

## Example usage:
- Save new loan application `https://localhost:7130/api/loan-applications` returns `201 Created`
```json
{
    "name": "a bc",
    "email": "test@gmail.com",
    "monthlyIncome": 1,
    "requestedAmount": 2,
    "termMonths": 4
}
```
<img width="1219" height="547" alt="image" src="https://github.com/user-attachments/assets/86c2705a-51eb-48df-a9b7-b3d7ba911fa5" />


- If request does not pass validation it is going to be rejected and `400` is going to be returned
Request with missing fields and incorrect values:
```json
{
    "name": "OnlyName",
    "email": "incorrect_email.com",    
    "requestedAmount": 6000,
    "termMonths": 0
}
```

Returns
```json
{
    "errors": [
        "Name and surname is required, found only one",
        "Email is not valid",
        "Monthly income cannot be null",
        "Term months has to be positive"
    ]
}
```
<img width="1234" height="611" alt="image" src="https://github.com/user-attachments/assets/9e25f2f5-e6da-4d58-b743-e143516d955d" />



Notes:
- Controller reads idempotency header directly; Service queries LoanApplication repository by indempotentId and if match is found, it is being returned.
- Srvice methods accept a `CancellationToken` which is checked at the end validation in `LoanApplicationService.Validate` as well as just before saving in `LoanApplicationService.SaveNewApplication`
- Errors are handled by `ResultWithMessage` class and railway apprach. If errors occur, appropriate message is added to Messages collection. Calling method then checks if result is Valid (`true` is there are no messages) and either exists or proceeds.

## `GET` method
- Calls `loanValidationService.GetById(id)`.
  - If not found returns `404 Not Found`.
<img width="1212" height="621" alt="image" src="https://github.com/user-attachments/assets/5f7b6a12-a46d-4cd4-a3b1-02893a9e40f5" />


  - Otherwise returns `200 OK` with the result.
  <img width="1236" height="557" alt="image" src="https://github.com/user-attachments/assets/00b2a5ac-33ff-4acc-8fa7-2ec13637e59a" />



# `LoanApplicationProcessor`

## Purpose
`LoanBackgroundService` - hosted service that runs every 60 seconds, obtains loan applications pending approval via `LoanApplicationRepository.GetByStatus` and evalauates eligibility based on set of rules in `LoanReviewService.ReviewLoanApplication`.
Each evaluated rule is saved in `DecisionLogEntry` table. 
Evaluation, saving decinsion log entries and loan application is done in a transaction which is going to be rolled back upon an error. 

Once loan application is processed, user will see status updated to either:

- Approved
<img width="1242" height="576" alt="image" src="https://github.com/user-attachments/assets/691c5187-d713-4c4a-a885-08feae7a1d74" />


- Rejected
<img width="1221" height="579" alt="image" src="https://github.com/user-attachments/assets/298eba46-7514-43d6-93ba-c1288561fe9c" />




## Debugging 
For debugging purpose a GET endpoint `decision-log-entries` was created. It will return all decision log entries

<img width="1227" height="781" alt="image" src="https://github.com/user-attachments/assets/17ba8773-c878-48b3-8924-3e62080fdaa7" />




### Architecture notes
- Some of the methods do not return `ResultWithMessage`. This is shorthand and in real life example every method would be using it.
- It is assumed that every message is an error, if given more time I would add other severities e.g. information, warning. 
- `LoanReviewService.ReviewLoanApplication` does more than it needs to. It should only evaluate rules, while a separate method should save decision log entries and loan applications. 
- There is minimal logging that ideally would be expanded in production.
- Loan appliation records are processed one at a time however if faced with large number of records I would change this to `SqlBulkCopy` and even go beyond that with processing loan applications parallel in batches.
- Added unit tests only for eligibility rules but ideally every method of every service should be covered by unit tests. 
- Values for eligibility rules are stored in appSettings so that they can be changed without modifying code. We could move rules themselves to appSettings and obtain them using rules engine. 
