# Yaoya

# 既存のアプリにMCPサーバーを搭載する
ModelContextProtocolをNugetから追加。

### XxxxService.csを変更する
[McpServerToolType]をクラスの上につける。
[McpServerTool, Description("指定した名前の商品を一覧から削除する")]のような属性を呼び出し対象のメソッドにつける。

### App.xaml.csを変更する
```
services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
```

### Yaoya.csproj
WinExeを直す。
```
<OutputType>Exe</OutputType>
```

### claude_desktop_config.json
```
{
  "mcpServers": {
    "yaoya": {
      "command": "C:/path/to/YaoyaApp.exe"
    }
  }
}
```
