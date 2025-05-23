## [6.0.3](https://github.com/informatievlaanderen/message-handling/compare/v6.0.2...v6.0.3) (2025-05-05)


### Bug Fixes

* **consumer-sql:** correct nuget package + update dependencies ([e179b53](https://github.com/informatievlaanderen/message-handling/commit/e179b53bfc36d6a7d97462e5086c650593688973))

## [6.0.2](https://github.com/informatievlaanderen/message-handling/compare/v6.0.1...v6.0.2) (2025-04-11)


### Bug Fixes

* **nuget:** correct dependencies packages ([d1f18a3](https://github.com/informatievlaanderen/message-handling/commit/d1f18a3aa3c625348ea6938d44f3803316916968))

## [6.0.1](https://github.com/informatievlaanderen/message-handling/compare/v6.0.0...v6.0.1) (2025-04-11)


### Bug Fixes

* **nuget:** correct packages content ([3cb5b88](https://github.com/informatievlaanderen/message-handling/commit/3cb5b8868a25810314e622aaa110b958bcd8afaf))

# [6.0.0](https://github.com/informatievlaanderen/message-handling/compare/v5.2.0...v6.0.0) (2025-04-07)


### Code Refactoring

* use renovate and nuget + update pipeline ([6e5444d](https://github.com/informatievlaanderen/message-handling/commit/6e5444d722adebb753031079ea56e12fe841904e))


### BREAKING CHANGES

* update to dotnet 9

# [5.2.0](https://github.com/informatievlaanderen/message-handling/compare/v5.1.0...v5.2.0) (2024-12-04)


### Features

* add OffsetOverride ([d1a7a37](https://github.com/informatievlaanderen/message-handling/commit/d1a7a37738ea1698db16f9e8676824b74e23d90a))

# [5.1.0](https://github.com/informatievlaanderen/message-handling/compare/v5.0.1...v5.1.0) (2024-08-09)


### Features

* add IConsumer.ConsumeContinuously overload ([f1ac10c](https://github.com/informatievlaanderen/message-handling/commit/f1ac10c2f289b08cd1b34795f5f077e267f76bf8))

## [5.0.1](https://github.com/informatievlaanderen/message-handling/compare/v5.0.0...v5.0.1) (2024-03-08)


### Bug Fixes

* **bump:** style to trigger build ([11ede59](https://github.com/informatievlaanderen/message-handling/commit/11ede597d027143d1dbd0a4c1d4459e27d5fa13b))

# [5.0.0](https://github.com/informatievlaanderen/message-handling/compare/v4.9.1...v5.0.0) (2024-03-08)


### Features

* move to dotnet 8.0.2 ([fa323ea](https://github.com/informatievlaanderen/message-handling/commit/fa323ea26a9936fb783f4f970a6911f60d1d5806))


### BREAKING CHANGES

* move to dotnet 8.0.2

## [4.9.1](https://github.com/informatievlaanderen/message-handling/compare/v4.9.0...v4.9.1) (2023-08-02)


### Bug Fixes

* add Key to IMessageSerializer.Deserialize ([42b6fce](https://github.com/informatievlaanderen/message-handling/commit/42b6fcea97e6aac7c21cba66a5f216945d08ffb4))
* add Key to IMessageSerializer.Deserialize ([67d52bf](https://github.com/informatievlaanderen/message-handling/commit/67d52bf9c097511d42be7325312f85bc779e72e9))

# [4.9.0](https://github.com/informatievlaanderen/message-handling/compare/v4.8.3...v4.9.0) (2023-08-02)


### Features

* add IMessageSerializer ([9ff7e85](https://github.com/informatievlaanderen/message-handling/commit/9ff7e85a7cd4e3f687713ef0e1ab0a220df786c4))
* add IMessageSerializer ([190838c](https://github.com/informatievlaanderen/message-handling/commit/190838caad6572b0eb082f46ef6dceeefca61301))

## [4.8.3](https://github.com/informatievlaanderen/message-handling/compare/v4.8.2...v4.8.3) (2023-06-04)


### Bug Fixes

* add index on date processed ([a2a5c3c](https://github.com/informatievlaanderen/message-handling/commit/a2a5c3c58be17e80539e401e9dbab1cf8ec35497))

## [4.8.2](https://github.com/informatievlaanderen/message-handling/compare/v4.8.1...v4.8.2) (2023-03-30)


### Bug Fixes

* bump awssdk packages ([0310291](https://github.com/informatievlaanderen/message-handling/commit/0310291dffa30dd95c752d4d8f48a46f961ad3bd))

## [4.8.1](https://github.com/informatievlaanderen/message-handling/compare/v4.8.0...v4.8.1) (2023-03-06)


### Bug Fixes

* make producer reliable ([fd3de6c](https://github.com/informatievlaanderen/message-handling/commit/fd3de6ca8a4d8c2f4b1d31b6400ae9e5fe5e884c))

# [4.8.0](https://github.com/informatievlaanderen/message-handling/compare/v4.7.2...v4.8.0) (2023-03-02)


### Bug Fixes

* namespace ([e958c90](https://github.com/informatievlaanderen/message-handling/commit/e958c9028f599605781875d52fa0ead5f292a5cb))


### Features

* customizable creating of AmazonSQSClient ([9efd2e8](https://github.com/informatievlaanderen/message-handling/commit/9efd2e86f9b0523704da4e60ca6cb99b0822ba62))

## [4.7.2](https://github.com/informatievlaanderen/message-handling/compare/v4.7.1...v4.7.2) (2023-02-17)


### Bug Fixes

* consumer commit if processed message exists ([d541ddb](https://github.com/informatievlaanderen/message-handling/commit/d541ddbd3e6d0b6fb80a9b440a05b699fc2493fc))

## [4.7.1](https://github.com/informatievlaanderen/message-handling/compare/v4.7.0...v4.7.1) (2023-02-13)


### Bug Fixes

* add overload to SqsJsonMessage.Map with custom JsonSerializer ([41f91e1](https://github.com/informatievlaanderen/message-handling/commit/41f91e165176360fd65b6506f4176840416f0504))
* move using to inside namespace ([7a72303](https://github.com/informatievlaanderen/message-handling/commit/7a7230311e23250b4bf4850ebdc83de81ba5bcf6))

# [4.7.0](https://github.com/informatievlaanderen/message-handling/compare/v4.6.1...v4.7.0) (2022-12-21)


### Bug Fixes

* correct consumer test ([cfd31d5](https://github.com/informatievlaanderen/message-handling/commit/cfd31d5013e5ae2c2c718ae81fb18f7e01ba3ac3))


### Features

* add consumer packages ([cf38934](https://github.com/informatievlaanderen/message-handling/commit/cf3893420c23ea6589a2d8afe03be7b7384ab49f))

## [4.6.1](https://github.com/informatievlaanderen/message-handling/compare/v4.6.0...v4.6.1) (2022-12-19)


### Bug Fixes

* style to trigger build ([0b3ecfd](https://github.com/informatievlaanderen/message-handling/commit/0b3ecfd4e12c2b4d627b857978ce54e412e53095))

# [4.6.0](https://github.com/informatievlaanderen/message-handling/compare/v4.5.0...v4.6.0) (2022-12-19)


### Bug Fixes

* change workflow & add to sonar ([239a3ac](https://github.com/informatievlaanderen/message-handling/commit/239a3ac36589b7ecc34280e37f60436f3e2cf6be))
* change workflow & add to sonar ([#141](https://github.com/informatievlaanderen/message-handling/issues/141)) ([aed8367](https://github.com/informatievlaanderen/message-handling/commit/aed8367f506a3c45a6f70f5a5b352ab4e20f4225))
* no sonar ([71ded8b](https://github.com/informatievlaanderen/message-handling/commit/71ded8b651c5d6094f639e3652efe6eef34efdf8))
* remove RabbitMq from sln ([3ea6cdc](https://github.com/informatievlaanderen/message-handling/commit/3ea6cdc8f3de0e554b07f2c320b830bb2fd59652))


### Features

* add kafka producer lib ([5cb7f77](https://github.com/informatievlaanderen/message-handling/commit/5cb7f77da1707fdbcb68a7069705071dd9fe996a))

# [4.5.0](https://github.com/informatievlaanderen/message-handling/compare/v4.4.0...v4.5.0) (2022-10-18)


### Features

* add multiple partition option ([cdf4c9c](https://github.com/informatievlaanderen/message-handling/commit/cdf4c9caacf1fb3f7f157c09f4f8d9c34ea82e56))

# [4.4.0](https://github.com/informatievlaanderen/message-handling/compare/v4.3.2...v4.4.0) (2022-10-18)


### Features

* add Kafka Produce method without serialization ([d2f18b3](https://github.com/informatievlaanderen/message-handling/commit/d2f18b323f2975acf4615e9396d7c3672f1f8917))

## [4.3.2](https://github.com/informatievlaanderen/message-handling/compare/v4.3.1...v4.3.2) (2022-09-22)


### Bug Fixes

* copy to queue requires url ([32ee174](https://github.com/informatievlaanderen/message-handling/commit/32ee174207045a5c848276366ac7bfabe5ae911c))

## [4.3.1](https://github.com/informatievlaanderen/message-handling/compare/v4.3.0...v4.3.1) (2022-09-21)


### Bug Fixes

* add position method to consumer ([c8d05b4](https://github.com/informatievlaanderen/message-handling/commit/c8d05b431e9c7d43e81b186fbdc718cc0186bffb))

# [4.3.0](https://github.com/informatievlaanderen/message-handling/compare/v4.2.3...v4.3.0) (2022-09-05)


### Features

* add deduplication id ([3a072cb](https://github.com/informatievlaanderen/message-handling/commit/3a072cbfa8bcef07e74a212a0d11339fa70bf056))

## [4.2.3](https://github.com/informatievlaanderen/message-handling/compare/v4.2.2...v4.2.3) (2022-07-08)


### Bug Fixes

* SqsQueueOptions to record ([562f7e5](https://github.com/informatievlaanderen/message-handling/commit/562f7e52f23010c4b0d4466d04d36f16f303ed82))

## [4.2.2](https://github.com/informatievlaanderen/message-handling/compare/v4.2.1...v4.2.2) (2022-07-08)


### Bug Fixes

* add SqsQueueOptions ([6f15411](https://github.com/informatievlaanderen/message-handling/commit/6f15411fcec9da6f30fcd44190e2819d5de82127))

## [4.2.1](https://github.com/informatievlaanderen/message-handling/compare/v4.2.0...v4.2.1) (2022-06-28)


### Bug Fixes

* allow messageGroupId in CopyToQueue ([b635a4d](https://github.com/informatievlaanderen/message-handling/commit/b635a4de7effeac6b2174cc3d0adb66247e667b6))

# [4.2.0](https://github.com/informatievlaanderen/message-handling/compare/v4.1.7...v4.2.0) (2022-06-24)


### Features

* add copy to queue & create queue if not exists ([20392d5](https://github.com/informatievlaanderen/message-handling/commit/20392d5932d00207d02e4fb9e53a707ef66b4443))

## [4.1.7](https://github.com/informatievlaanderen/message-handling/compare/v4.1.6...v4.1.7) (2022-06-22)


### Bug Fixes

* add ctor without credentials ([5d4132f](https://github.com/informatievlaanderen/message-handling/commit/5d4132f4b8bf230decc9cb7269ca19046092ad92))

## [4.1.6](https://github.com/informatievlaanderen/message-handling/compare/v4.1.5...v4.1.6) (2022-06-21)


### Bug Fixes

* remove region endpoint ([32597b5](https://github.com/informatievlaanderen/message-handling/commit/32597b54b571d3c6536827981ecbe22cc248b91e))

## [4.1.5](https://github.com/informatievlaanderen/message-handling/compare/v4.1.4...v4.1.5) (2022-06-21)


### Bug Fixes

* add basic auth ([83a627c](https://github.com/informatievlaanderen/message-handling/commit/83a627cb4e7e78f055700bacaa5a72664f7dfd65))

## [4.1.4](https://github.com/informatievlaanderen/message-handling/compare/v4.1.3...v4.1.4) (2022-06-08)


### Bug Fixes

* change AWS SQS call interface ([3890d6f](https://github.com/informatievlaanderen/message-handling/commit/3890d6f4848a884605535277c7e82f1c5bd08f2a))

## [4.1.3](https://github.com/informatievlaanderen/message-handling/compare/v4.1.2...v4.1.3) (2022-06-08)


### Bug Fixes

* correct build.fsx ([752c7af](https://github.com/informatievlaanderen/message-handling/commit/752c7af45dcb5979271cc137f89ac33a51ce4d6b))

## [4.1.2](https://github.com/informatievlaanderen/message-handling/compare/v4.1.1...v4.1.2) (2022-06-08)


### Bug Fixes

* add sqs lib to nuget ([5772edb](https://github.com/informatievlaanderen/message-handling/commit/5772edb61a1b46ae3903f8824243a10df44df1d3))

## [4.1.1](https://github.com/informatievlaanderen/message-handling/compare/v4.1.0...v4.1.1) (2022-06-08)


### Bug Fixes

* fix build.fsx ([8c8e19b](https://github.com/informatievlaanderen/message-handling/commit/8c8e19bf5f0916323f42de25b29010da0bc13bfb))

# [4.1.0](https://github.com/informatievlaanderen/message-handling/compare/v4.0.0...v4.1.0) (2022-06-08)


### Features

* add AWS SQS support ([5f7da68](https://github.com/informatievlaanderen/message-handling/commit/5f7da6865ab37524476d29de855978ab8eadf7e9))

# [4.0.0](https://github.com/informatievlaanderen/message-handling/compare/v3.0.0...v4.0.0) (2022-05-13)


### Features

* add delay when no message was found ([52bcb9e](https://github.com/informatievlaanderen/message-handling/commit/52bcb9e5638a37ca74edc27ece44dadcbb7bd2dd))


### BREAKING CHANGES

* refactored the options into producer & consumer options

# [3.0.0](https://github.com/informatievlaanderen/message-handling/compare/v2.1.0...v3.0.0) (2022-04-06)


### Features

* add consumption from specific offset ([da870bd](https://github.com/informatievlaanderen/message-handling/commit/da870bd98185740c9847f2fa0fa5440eb915bd25))


### BREAKING CHANGES

* Consumer now returns `Result<KafkaJsonMessage>` instead of `Result`

# [2.1.0](https://github.com/informatievlaanderen/message-handling/compare/v2.0.0...v2.1.0) (2022-04-05)


### Features

* add kafka topic management ([d0d13ae](https://github.com/informatievlaanderen/message-handling/commit/d0d13aefa43c0210a59ba3cba099a12dcc6cd608))

# [2.0.0](https://github.com/informatievlaanderen/message-handling/compare/v1.3.2...v2.0.0) (2022-03-28)


### Features

* move to dotnet 6.0.3 ([aa04c41](https://github.com/informatievlaanderen/message-handling/commit/aa04c4124d3b1519b74216df64ef33bf68517a9c))


### BREAKING CHANGES

* move to dotnet 6.0.3

## [1.3.2](https://github.com/informatievlaanderen/message-handling/compare/v1.3.1...v1.3.2) (2022-03-02)


### Bug Fixes

* cleanup kafka auth ([8ce39f7](https://github.com/informatievlaanderen/message-handling/commit/8ce39f71e658433928af6c930c0db1cf048a9430))
* fix kafka authn ([f057a24](https://github.com/informatievlaanderen/message-handling/commit/f057a24fb13590d467dd34981f6e0c8b3e803c69))
* fix KafkaAuthentication ([#72](https://github.com/informatievlaanderen/message-handling/issues/72)) ([1774517](https://github.com/informatievlaanderen/message-handling/commit/177451776b2739f65764976696845993871fab64))

## [1.3.1](https://github.com/informatievlaanderen/message-handling/compare/v1.3.0...v1.3.1) (2022-02-17)


### Bug Fixes

* add kafka authentication extensions ([06366cc](https://github.com/informatievlaanderen/message-handling/commit/06366cc2aee9ff5dbaf16ed263f629b0280164c4))

# [1.3.0](https://github.com/informatievlaanderen/message-handling/compare/v1.2.4...v1.3.0) (2022-02-16)


### Features

* add Sasl authentication options ([5030daf](https://github.com/informatievlaanderen/message-handling/commit/5030daf777be6b3ae70b0d52c87defa431328f0e))

## [1.2.4](https://github.com/informatievlaanderen/message-handling/compare/v1.2.3...v1.2.4) (2022-01-28)


### Bug Fixes

* consume now accepts object ([380333f](https://github.com/informatievlaanderen/message-handling/commit/380333fa2079cc0d718f8e7c927a5bc64c51b368))

## [1.2.3](https://github.com/informatievlaanderen/message-handling/compare/v1.2.2...v1.2.3) (2022-01-27)


### Reverts

* Revert "feature: kafka json message serdes" ([e99684f](https://github.com/informatievlaanderen/message-handling/commit/e99684f8ee15671ff2fb326a50a44076b31e7656))

## [1.2.2](https://github.com/informatievlaanderen/message-handling/compare/v1.2.1...v1.2.2) (2022-01-27)


### Bug Fixes

* fix build.fsx ([0b8306b](https://github.com/informatievlaanderen/message-handling/commit/0b8306bf5169134108a45dcb239d23c181d1aca3))
* readd file correct naming ([88e7867](https://github.com/informatievlaanderen/message-handling/commit/88e7867ea9a12e9a0f11e79e070d3b57d8a0fc34))

## [1.2.1](https://github.com/informatievlaanderen/message-handling/compare/v1.2.0...v1.2.1) (2022-01-27)


### Bug Fixes

* change build to not run tests ([7af7676](https://github.com/informatievlaanderen/message-handling/commit/7af7676e9c47e276798a056c749b8c55254b23b8))

# [1.2.0](https://github.com/informatievlaanderen/message-handling/compare/v1.1.5...v1.2.0) (2022-01-26)


### Bug Fixes

* add KafkaOptions.cs ([2a244f2](https://github.com/informatievlaanderen/message-handling/commit/2a244f26052ba9a1327d586773e9254e1d43a6ee))
* change namespace ([#49](https://github.com/informatievlaanderen/message-handling/issues/49)) ([dc77b36](https://github.com/informatievlaanderen/message-handling/commit/dc77b363883748566d02887ffef36d54043f762a))
* create KafkaOptions.cs ([ed2bc10](https://github.com/informatievlaanderen/message-handling/commit/ed2bc1015fb0cc676b7129ff321280cd4d92283c))
* remove LibTest from build.fsx ([fc97169](https://github.com/informatievlaanderen/message-handling/commit/fc9716960a79f36a148fb6ef320f6c92e5fd9f34))
* remove test project from build.fsx ([b9f3b98](https://github.com/informatievlaanderen/message-handling/commit/b9f3b98e61a5602ff9275a596c9f73239a039a18))


### Features

* add kafka options ([d354284](https://github.com/informatievlaanderen/message-handling/commit/d35428448b7209ac4d9c7f87ed64e6d7d16b02ae))

## [1.1.5](https://github.com/informatievlaanderen/message-handling/compare/v1.1.4...v1.1.5) (2022-01-24)


### Bug Fixes

* switch build to kafka only ([cb30bb9](https://github.com/informatievlaanderen/message-handling/commit/cb30bb9299df0a5be6802fb13e5033b3339aef7e))

## [1.1.4](https://github.com/informatievlaanderen/message-handling/compare/v1.1.3...v1.1.4) (2022-01-24)


### Bug Fixes

* remove kafka from build to try build again ([b7dc412](https://github.com/informatievlaanderen/message-handling/commit/b7dc412bdbd2de39d9ba5331ca8a06db26b5f8f8))

## [1.1.3](https://github.com/informatievlaanderen/message-handling/compare/v1.1.2...v1.1.3) (2022-01-24)


### Bug Fixes

* style to trigger build ([689fcfb](https://github.com/informatievlaanderen/message-handling/commit/689fcfb21c8be2c4f733e69ca1377f6c04d60e40))

## [1.1.2](https://github.com/informatievlaanderen/message-handling/compare/v1.1.1...v1.1.2) (2022-01-24)


### Bug Fixes

* style to trigger build ([c32094e](https://github.com/informatievlaanderen/message-handling/commit/c32094e301477bd693053ede22399bbf2b2c2f03))

## [1.1.1](https://github.com/informatievlaanderen/message-handling/compare/v1.1.0...v1.1.1) (2022-01-24)


### Bug Fixes

* publish kafka simple to nuget ([7f58909](https://github.com/informatievlaanderen/message-handling/commit/7f5890994ec0d23fc97b1fc88125b628c4eb1b18))

# [1.1.0](https://github.com/informatievlaanderen/message-handling/compare/v1.0.7...v1.1.0) (2022-01-24)


### Bug Fixes

* add kafka lib to build ([905e874](https://github.com/informatievlaanderen/message-handling/commit/905e874f867cee11d01f5d7f6b2bf2f77fdd6fd8))


### Features

* add kafka simple lib ([c4bf57b](https://github.com/informatievlaanderen/message-handling/commit/c4bf57b4237fa70784755e41547e303b2aadd226))

## [1.0.7](https://github.com/informatievlaanderen/message-handling/compare/v1.0.6...v1.0.7) (2022-01-24)


### Bug Fixes

* build rabbitmq but don't push to nuget ([04b5ed8](https://github.com/informatievlaanderen/message-handling/commit/04b5ed8897f12a02e602bc551e82b0c44c3b885f))
* clean up + fix build ([ff697db](https://github.com/informatievlaanderen/message-handling/commit/ff697db68e77af182065b08b1ea94ca6b9ebdde9))
* revert to last working build ([58d03f6](https://github.com/informatievlaanderen/message-handling/commit/58d03f60c0ca66859ed893f09bbc81a243258dae)), closes [#36](https://github.com/informatievlaanderen/message-handling/issues/36) [#35](https://github.com/informatievlaanderen/message-handling/issues/35)

## [1.0.3](https://github.com/informatievlaanderen/message-handling/compare/v1.0.2...v1.0.3) (2021-11-17)

## [1.0.2](https://github.com/informatievlaanderen/message-handling/compare/v1.0.1...v1.0.2) (2021-11-15)


### Bug Fixes

* use polly for retries & fix bugs ([da7c4be](https://github.com/informatievlaanderen/message-handling/commit/da7c4beb6a4fba83ac9f2bbbdea7b89367c15d04))

## [1.0.1](https://github.com/informatievlaanderen/message-handling/compare/v1.0.0...v1.0.1) (2021-11-09)


### Bug Fixes

* use `npm ci` instead of install ([74f7092](https://github.com/informatievlaanderen/message-handling/commit/74f709240079b323d7f7af996c7e7b945a54b216))

# 1.0.0 (2021-11-09)


### Features

* add rabbitmq ([1535b3e](https://github.com/informatievlaanderen/message-handling/commit/1535b3eb113648d80e85dba6cc355d9b5343afee))
