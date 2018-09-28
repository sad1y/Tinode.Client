create/update gRPC proxy class

note: updated metadate file can be obtained from https://github.com/tinode/chat/blob/master/pbx/model.proto
 
```~/.nuget/packages/grpc.tools/1.15.0/tools/linux_x64/protoc --csharp_out . --grpc_out . --plugin=protoc-gen-grpc=/home/zoth/.nuget/packages/grpc.tools/1.15.0/tools/linux_x64/grpc_csharp_plugin ./tinode.proto```
