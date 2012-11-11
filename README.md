Integration.CommitService
=========================

Web Service based on ServiceStack.NET for accepting post-commit hook messages from remote source control repositories

# Work in progress

This project is not ready for use just yet with VersionOne. It's still being baked, but we're opening up more of our tools and source code for external visibility and contributions. We welcome pull-requests, especially for additional implementations of [ITranslateCommitAttempt](https://github.com/versionone/Integration.CommitService/blob/master/CommitService.Contract/ITranslateCommitAttempt.cs) for additional source-code repositories.

# Built on open source

It is being built on top of the open source [ServiceStack](http://www.servicestack.net/) project. Many thanks already to [Demis Bellot](https://github.com/mythz) for his assistance in the [ServiceStack chat room in Jabbr](http://jabbr.net/#/rooms/servicestack)

The current implementation also uses the popular open source [redis NoSql key-value store](http://redis.io/) for message queueing, as [explained by ServiceStack's tutorial examples](https://github.com/ServiceStack/ServiceStack/wiki/Messaging-and-redis).

# Further reading

We highly recommend reading [Advantages of message based web services](https://github.com/ServiceStack/ServiceStack/wiki/Advantages-of-message-based-web-services) from the ServiceStack wiki. It gives a great background on the theory and the practical reasons for simple messaging. It covers the history and current state of web messaging in use by Google, Amazon, Twitter, Microsoft, etc. Microsoft does not get off easy, however. It highlights the frustration that developers have faced when attempting to go "by the book" from many tightly-coupled frameworks that Microsoft has produced. Instead, it describes how loose-coupling and thin, POCO classes present great advantages. It shows how to benefit from generic interfaces (the true nature of REST), common client code, and digs a bit into examples using jQuery and Backbone.js as well. It's good stuff.

You may also find these useful:

* http://www.eaipatterns.com/
* http://servicedesignpatterns.com/