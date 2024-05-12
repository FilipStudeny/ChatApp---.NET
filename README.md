# Chat app

Simple chat app creted using **.NET**, **Flutter**, **MongoDB** and **React**. Contains tests for backend API

## Tech stack

| Frontend         | Backend | Rest      |
| ---------------- | ------- | --------- |
| React            | .NET    | xUnit     |
| Flutter (mobile) | MongoDB | Mongo2Go  |
|                  |         | React Mui |
|                  |         | JWT       |

## Project structure

### /Backend

- **API** - Contains app logic
- **Shared** - Contains shared models/service betwean tests and API
- **Tests** - Contains tests, Uses **xUnit**, **NSubstitue** and **Mongo2Go** for as testing suite

### /Frontend

Uses React for the web Flutter for the phone version

- **React**
  - Redux
  - ReactQuery
  - Mui
- **Flutter (mobile)**

## Planned features

- 100% test coverage
- Sending files/images/videos
- Screen sharing
- Video calls
- Chat groups
- Mobile version using Flutter
