[![Build status](https://ci.appveyor.com/api/projects/status/6d1wt5hjaag1qxbh?svg=true)](https://ci.appveyor.com/project/aoancea/practice-web-api-security)

# practice-web-api-security

## Purpose
The purpose of this tutorial has several important parts:
  * It shows the Token Based Authentication using OWIN middleware pipeline with ASP.NET Web Api 2. This is mostly a copy of an already existing **GREAT** tutorial provided by [Taiseer Joudeh](https://github.com/tjoudeh) called [AngularJSAuthentication](https://github.com/tjoudeh/AngularJSAuthentication). You can also find more about it on his blog [here](http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/)
  * ~~It wants to provide integration with a really cool open-source project hosted here on GitHub called [Satellizer](https://github.com/sahat/satellizer)~~
  * It also provides the foundation for me to play with these technologies and code mentioned above and to input some of my own thoughts

## Technical details
We are going to use different tools and technologies. The system will be split into two:
  * Api: a RESTFUL Api providing the data(we are going to secure this)
  * Web: the Client of the Api which is going to present the information to the viewer(mainly build with Angular 1.x)

We'll be providing extra security using the Anti-Forgery Token. This will validate each request given that the Web app provides it.

You can have a look at the **OAuth 2** standard that we'll be using [here](https://tools.ietf.org/html/rfc6749). We'll be using the **Resource Owner Password Credentials Grant** which is explained [here](http://tools.ietf.org/html/rfc6749#section-4.3).

Source code of the **OWIN middleware** aka ***Katana Project*** can be found on [codeplex](https://katanaproject.codeplex.com/SourceControl/latest#README).

### Technology stack
 * ASP.NET Web Api
 * ASP.NET MVC
 * OWIN, OAuth 2
 * HTML, CSS, Javascript
 * Angular 1.x
 * MsSql Server
 * Entity Framework
 * Bootstrap


## Resources
 * http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
 * https://tools.ietf.org/html/rfc6749
 * http://stackoverflow.com/questions/6269376/oauth-what-exactly-is-a-resource-owner-when-is-it-not-an-end-user

## Issuing an Access Token
Access tokens are being issued by the ***authorization server*** if the request is valid and authorized. There are several ways in which you can send an authorized request and in the following lines I will present what is supported here

### Resource Owner Password Credentials Grant
This implies that the client trying to obtain an ***access_token*** holds a ***username*** and ***password***. Upon usage an authorized request is created towards the ***authorization server*** which in return will issue the ***access_token*** that will be used by the client to authenticate against the ***resource server***.

More information related to this can be found [here](https://tools.ietf.org/html/rfc6749#section-4.3). There are also some security considerations outlined [here](https://tools.ietf.org/html/rfc6749#section-10.7)

In order for a client to ask for an ***access_token*** a request must be issued to the **'/token'** endpoint with the following format:

```
Headers
  POST /token HTTP/1.1
  Host: server.example.com
  Content-Type: application/x-www-form-urlencoded;
Body
  grant_type = password
  username = test
  password = testpassword
```
