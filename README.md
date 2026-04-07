# Yaoya

# 既存のアプリにMCPサーバーを搭載する
ModelContextProtocolをNugetから追加。

### YaoyaMcpTools.csを作成する
[McpServerToolType]をクラスの上につける。  
[McpServerTool, Description("指定した名前の商品を一覧から削除する")]のような属性を呼び出し対象のメソッドにつける。  
必要なら、引数にもDescriptionを付ける。 [Description("Mathematical expression to evaluate (e.g., '5 + 3', '10 - 2', '4 * 6', '15 / 3')")] string expression

### App.xaml.csを変更する
WithToolsFromAssemblyだと、AIがツールとして取得してくれないみたい。
```
services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<YaoyaMcpTools>();
```
注意として、ここで指定されたクラス（YaoyaMcpTools）はシングルトン登録しててもMCP用にインスタンスが作られてしまうので、このクラスにデータを持たせると更新不整合が発生する。  
あくまでYaoyaMcpToolsはMCP処理の入り口にして、シングルトン登録したサービスを注入して使うこと。

### Yaoya.csproj
WinExeを直す。
```
<OutputType>Exe</OutputType>
```

### claude_desktop_config.json
EXEの配置場所をcommandに書く。  
これを書くとClaude Desktop起動時に勝手にソフトが起動するので、嫌な場合はバックグラウンドでMCP受信と仲介を行うような代理のソフトを作るしかない。
```
{
  "mcpServers": {
    "yaoya": {
      "command": "C:/path/to/YaoyaApp.exe"
    }
  }
}
```
