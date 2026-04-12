# Quick Guide
Outputs are data objects that may or may not contain data to return a caller. These can be returned to provide error or exception related information to callers in a more accessible manner rather than throwing exceptions in an application at runtime.

To this end, the library provides a string parseable `OutputCode` for outputs. The current format for these codes are in the form `{status}.{detail}`, where the status is a strongly-named `OutputStatus` and the detail is a strongly-named `DetailCode`. These can be extended upon in any application needing to define unique and custom tailored status or detail names. Detail codes are meant to provide a more granular information to a status code. For example, an output status might be `InvalidRequest` but to help provide more contextual information to a caller you could set an additional detail code for `Identifier` that would then read as `There was an invalid request due to an identifier in the request payload.` OutputStatuses are unique for method functions but they are setup to share commonality with many of the standard HTTPStatusCodes that currently exist, to help provide more flexibility and ease of use with applications.

A brief summary of the currently support output types:
- `Output`: a basic output that provides an output code
- `Output<T>`: an output that returns stored data to a caller
- `PaginatedOutput`: represents an output that supports pagination, that is there could be more items than returned in the current output that a caller should be sure to include if needed.
- `MultiOutput`: an output that is compromised of one or more other outputs

# Status and Detail Codes
The standard status codes do share similarities with HTTP Status Codes, allowing for custom statuses per domain as needed, similar to detail codes. To help prevent as much collision as possible with this library and others, three digit codes between 600-999 are not foreseen to be used and are freely available to be used without concern for collision. Furthermore, to help set a known boundary, the first 200 codes should be reserved for this libraries potential needs. There may be a need to set library codes between 200-600 for other HTTP Status Codes or library needs, but it is not currently anticipated.

If more codes are needed in the event of some unforeseen design change, the library mechanisms for outpus will allow 4 or more digit codes, and further notice on this topic will be provided should such need arise.

# Usage
The main entry point into the libray is the `Out` static factory. This is used to create outputs in a more accessible way to callers. 

Additionally, if collecting diagnostic information during output generation is desired, you can use the `CreateDiagnosticScope` method. This will create a scope for diagnostic collection and provide related information during output returns from the `Out` static factory. The scope is disposable and should be disposed once diagnostic collection is no longer needed.