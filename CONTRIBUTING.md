# Contributing to Vehicle Manager

Thank you for your interest in contributing to Vehicle Manager! This document provides guidelines and information for contributors.

## 🤝 How to Contribute

### Reporting Issues

1. **Search existing issues** first to avoid duplicates
2. **Use the issue template** when creating new issues
3. **Provide detailed information** including:
   - Operating system and version
   - .NET version
   - Steps to reproduce
   - Expected vs actual behavior
   - Screenshots if applicable

### Submitting Pull Requests

1. **Fork** the repository
2. **Create a feature branch** from `develop`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes** following our coding standards
4. **Test your changes** thoroughly
5. **Commit your changes** with descriptive messages:
   ```bash
   git commit -m "Add: New vehicle search functionality"
   ```
6. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```
7. **Create a Pull Request** to the `develop` branch

## 📋 Development Guidelines

### Code Style

- **Follow C# conventions** and use PascalCase for public members
- **Use meaningful names** for variables, methods, and classes
- **Add XML documentation** for public APIs
- **Keep methods small** and focused on a single responsibility
- **Use async/await** for database operations

### Project Structure

```
src/
├── VehicleManager.Core/          # Shared business logic
│   ├── Models/                   # Data models
│   ├── Data/                     # Database access layer
│   └── ViewModels/               # MVVM ViewModels
├── VehicleManager.WPF/           # WPF UI implementation
└── VehicleManager.Avalonia/      # Cross-platform UI
```

### Commit Message Format

Use conventional commit messages:

- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `style:` Formatting changes
- `refactor:` Code refactoring
- `test:` Adding tests
- `chore:` Maintenance tasks

Example: `feat: Add vehicle search by license plate`

### Testing

- **Write unit tests** for new functionality
- **Ensure all tests pass** before submitting PR
- **Test on multiple platforms** when possible
- **Include integration tests** for database operations

## 🛠️ Development Setup

### Prerequisites

- .NET 8 SDK
- Git
- IDE: Visual Studio, VS Code, or JetBrains Rider
- MySQL (optional - offline mode available)

### Local Development

1. **Clone your fork**:
   ```bash
   git clone https://github.com/YOUR_USERNAME/vehicle-manager.git
   cd vehicle-manager
   ```

2. **Install dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

4. **Run tests**:
   ```bash
   dotnet test
   ```

5. **Start the application**:
   ```bash
   dotnet run --project src/VehicleManager.Avalonia/VehicleManager.Avalonia
   ```

### Database Setup for Development

For local development, you can use either:

1. **Offline mode** (default) - Uses in-memory data
2. **MySQL** - Set up local MySQL instance

To switch to MySQL:
1. Update connection string in `VehicleDatabase.cs`
2. Change `MainViewModel.cs` to use `VehicleDatabase` instead of `OfflineVehicleDatabase`

## 📝 Code Review Process

1. **Automated checks** must pass (build, tests, formatting)
2. **At least one reviewer** approval required
3. **Maintainer review** for significant changes
4. **Squash and merge** preferred for cleaner history

## 🎯 Areas for Contribution

We welcome contributions in these areas:

### High Priority
- [ ] Unit tests for core functionality
- [ ] PDF export implementation
- [ ] Better error handling and user feedback
- [ ] Performance improvements

### Medium Priority
- [ ] Localization/internationalization
- [ ] Dark theme support
- [ ] Vehicle image upload/display
- [ ] Advanced search filters

### Low Priority
- [ ] Data export (CSV, Excel)
- [ ] Backup/restore functionality
- [ ] Plugins/extensions system
- [ ] Mobile app version

## 🐛 Bug Reports

When reporting bugs, please include:

- **Clear description** of the issue
- **Steps to reproduce** the problem
- **Expected behavior** vs actual behavior
- **Environment details** (OS, .NET version, etc.)
- **Screenshots** or error messages
- **Log files** if available

## 💡 Feature Requests

For new features:

- **Describe the use case** and why it's needed
- **Provide examples** of how it should work
- **Consider backwards compatibility**
- **Think about cross-platform implications**

## 📚 Documentation

Help improve documentation by:

- **Fixing typos** and grammar errors
- **Adding examples** and tutorials
- **Improving API documentation**
- **Translating to other languages**

## 🏷️ Release Process

1. **Features** merged to `develop` branch
2. **Release candidate** created from `develop`
3. **Testing phase** on multiple platforms
4. **Release** merged to `main` and tagged
5. **Artifacts** published to GitHub Releases

## ❓ Questions?

- **Create an issue** for general questions
- **Discussion tab** for ideas and brainstorming
- **Wiki** for additional documentation

## 📄 License

By contributing to Vehicle Manager, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to Vehicle Manager! 🚗✨