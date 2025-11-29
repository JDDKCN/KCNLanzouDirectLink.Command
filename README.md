<div align="center"><strong>

# KCNLanzouDirectLink.Command

</strong></div>

一个基于 [KCNLanzouDirectLink](https://www.nuget.org/packages/KCNLanzouDirectLink) 类库的命令行工具，用于解析蓝奏云分享链接的直链。

## 软件下载

### 下载二进制分发文件 (开箱即用)
请到最新 [**Releases**](https://github.com/JDDKCN/KCNLanzouDirectLink.Command/releases/) 处下载。

### 通过源代码构建
请下载 [**项目源码**](https://github.com/JDDKCN/KCNLanzouDirectLink.Command/archive/refs/heads/main.zip) 自行编译，需要 `Visual Studio` 与 `.NET 9` 开发环境。

## 使用方法

### 获取直链

#### 参数

| 参数 | 缩写 | 必填 | 默认值 | 说明 |
|------|------|------|--------|------|
| `link` | - | ✅ | - | 代表获取直链 |
| `--url` | `-u` | ✅ | - | 蓝奏云分享链接 |
| `--password` | `-p` | ❌ | - | 分享密码 |
| `--retry` | `-r` | ❌ | 3 | 重试次数 |
| `--json` | `-j` | ❌ | false | JSON 格式输出 |
| `--quiet` | `-q` | ❌ | false | 静默模式，仅输出直链 |

#### 示例

```bash
# 普通链接
KCNLanzouDirectLink.Command link -u "https://lanzou.com/xxxxx"

# 加密链接
KCNLanzouDirectLink.Command link -u "https://lanzou.com/xxxxx" -p "1234"

# 静默模式，仅输出直链
KCNLanzouDirectLink.Command link -u "https://lanzou.com/xxxxx" -q

# JSON 格式输出
KCNLanzouDirectLink.Command link -u "https://lanzou.com/xxxxx" -j
```

### 获取文件信息

#### 参数

| 参数 | 缩写 | 必填 | 默认值 | 说明 |
|------|------|------|--------|------|
| `info` | - | ✅ | - | 代表获取信息 |
| `--url` | `-u` | ✅ | - | 蓝奏云分享链接 |
| `--password` | `-p` | ❌ | - | 分享密码 |
| `--json` | `-j` | ❌ | false | JSON 格式输出 |

#### 示例

```bash
# 获取普通文件信息
KCNLanzouDirectLink.Command info -u "https://lanzou.com/xxxxx"

# 获取加密文件信息
KCNLanzouDirectLink.Command info -u "https://lanzou.com/xxxxx" -p "1234"
```

### 批量获取

#### 参数

| 参数 | 缩写 | 必填 | 默认值 | 说明 |
|------|------|------|--------|------|
| `batch` | - | ✅ | - | 代表批量获取 |
| `--file` | `-f` | ✅ | - | 链接文件路径 |
| `--output` | `-o` | ❌ | - | 结果输出文件路径 |
| `--retry` | `-r` | ❌ | 3 | 重试次数 |
| `--json` | `-j` | ❌ | false | JSON 格式输出 |
| `--delay` | `-d` | ❌ | 500 | 请求间隔 (ms) |

#### 链接文件格式

```text
# 这是注释
https://lanzou.com/xxxxx
https://lanzou.com/yyyyy,1234
https://lanzou.com/zzzzz	password
```

支持的分隔符：逗号 `,`、制表符、空格

#### 示例

```bash
# 批量处理
KCNLanzouDirectLink.Command batch -f urls.txt

# 保存结果到文件
KCNLanzouDirectLink.Command batch -f urls.txt -o results.txt

# JSON 格式输出
KCNLanzouDirectLink.Command batch -f urls.txt -o results.json -j

# 自定义延迟和重试
KCNLanzouDirectLink.Command batch -f urls.txt -d 1000 -r 5
```

## 退出码

| 退出码 | 说明 |
|--------|------|
| 0 | 成功 |
| 1 | 失败或部分失败 |

## 系统支持

| 系统 | 可用性 | 支持版本 |
|------|--------|---------|
| Windows | 支持 | x86、x64、Arm、Arm64 |
| Linux | 支持 | x64、Arm、Arm64 |
| Mac OS | 支持 | x64、Arm64 |

> Release 中二进制文件仅提供 Windows x86_64 版本，使用其他版本请自行编译。

## 许可证与免责声明
本项目基于 [AGPL-3.0](LICENSE) 许可证开源。本项目仅供研究交流用，禁止用于商业及非法用途。使用本项目造成的事故与损失，与作者无关。本项目完全免费，如果您是花钱买的，说明您被骗了。请尽快退款，以减少您的损失。

## 其他平台
前往我的 [**B站主页**](https://space.bilibili.com/475547854/) : 剧毒的KCN

关注我的 [**Twitter账号**](https://twitter.com/2233KCN03) : @2233kcn03

加入我的 [**QQ交流群**](http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=_-W8U_Mrz_nOu3eD_u3VGiPICKe9t7zY&authKey=rB2PW5mIrIY3ARjMqqWtw%2F2Qpejm5EArmuy95Wq1GfC7gLzUzTRATTnULKUKtb76&noverify=0&group_code=1140538395) : 1140538395