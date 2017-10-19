# Example for Why can I not mix Guid and string members in a ComVisible struct?

Build, then run the commands in Register.bat as administrator mutatis mutandis to register.  Run the server first as a regular user, then run the client.  If the struct has both a guid and string member and is returned as an array, I experience an exception in the server process when attempting to marshal from managed to unmanaged memory.

Remove either of the struct members or change the return value to not be an array, reregister, and run the example again and it will not fail.

Line generating error: [OleVariant::CreateSafeArrayDescriptorForArrayRef in src/vm/olevariant.cpp:4363](https://github.com/dotnet/coreclr/blob/46ab1d132c9ad471d79afa20c188c2f9c85e5f20/src/vm/olevariant.cpp#L4363)