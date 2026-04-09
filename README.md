# Yaoya

# 既存のアプリにMCPサーバーを搭載する
ModelContextProtocolをNugetから追加。

## YaoyaMcpTools.csを作成する
[McpServerToolType]をクラスの上につける。  
[McpServerTool, Description("指定した名前の商品を一覧から削除する")]のような属性を呼び出し対象のメソッドにつける。  
必要なら、引数にもDescriptionを付ける。 [Description("Mathematical expression to evaluate (e.g., '5 + 3', '10 - 2', '4 * 6', '15 / 3')")] string expression

## App.xaml.csを変更する
### ツールの公開
WithToolsFromAssemblyだと、AIがツールとして取得してくれないみたい。
```
services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<YaoyaMcpTools>();
```
注意として、ここで指定されたクラス（YaoyaMcpTools）はシングルトン登録しててもMCP用にインスタンスが作られてしまうので、このクラスにデータを持たせると更新不整合が発生する。  
あくまでYaoyaMcpToolsはMCP処理の入り口にして、シングルトン登録したサービスを注入して使うこと。

### ログをstderrに逃がす
MCPはstdoutをJSON-RPCの通信専用に使うから、ログが混入したら落ちてしまう。
```
using Microsoft.Extensions.Logging;

_host = Host.CreateDefaultBuilder(e.Args)
    .ConfigureAppConfiguration(c =>
    {
        c.SetBasePath(appLocation);
    })
    .ConfigureLogging(logging =>
    {
        // 全ログをstderrに向ける（stdoutはMCP専用にする）
        logging.AddConsole(options =>
        {
            options.LogToStandardErrorThreshold = LogLevel.Trace;
        });
    })
    .ConfigureServices(ConfigureServices)
    .Build();
```

## Yaoya.csproj
WinExeを直す。（stdioの場合のみ）
```
<OutputType>Exe</OutputType>
```

一応、Yaoya.csprojのターゲットフレームワークを```net10.0-windows```に変更する。

## claude_desktop_config.json

### stdioの場合
EXEの配置場所をcommandに書く。  
これを書くとClaude Desktop起動時に勝手にソフトが起動するので、嫌な場合はバックグラウンドでMCP受信と仲介を行うような代理のソフトを作るしかない。
この問題はStdioで接続しているためで、HTTP/SSEで通信する方式に変えたら解決するはず。
```
{
  "mcpServers": {
    "yaoya": {
      "command": "C:/path/to/YaoyaApp.exe"
    }
  }
}
```

### HTTP/SSEの場合
WPFはIHostなので、KestrelのHTTPサーバを自前で立てる必要がある。  
Microsoft.AspNetCore.AppをNugetで導入。  
Claude DesktopはローカルHTTP/SSEに直接対応していないため、mcp-remoteを使用する。  
mcp-remoteを使用するにはNode.jsのインストールが必要。
```
{
  "mcpServers": {
    "yaoya": {
      "command": "npx",
      "args": [
        "mcp-remote",
        "http://localhost:5000/sse"
      ]
    }
  }
}
```

# 問題点
HTTP/SSEに対応させようとすると、KestrelとWPFのUIスレッドが競合してしまう。  
現在のソースのようにスレッドを作れば回避できるが、何故かタイトルバーのドラッグができなくなる。（ShellWindow.xaml.csを変更することで無理矢理回避できるが、納得いかない）  
WPFをやめてBlazor Desktopで作るとか、MCP対応はコンソールアプリだけにするとか、そんな感じで考えよう…。




