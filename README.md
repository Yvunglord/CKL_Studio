# CKL Studio: Инструментальная система для построения алгебры динамических отношений

<div align="center">
  
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-%3E%3D%207.0-blue)](https://dotnet.microsoft.com/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Yvunglord_CKL_Studio&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Yvunglord_CKL_Studio)
[![WPF](https://img.shields.io/badge/UI-WPF-ff69b4)](https://docs.microsoft.com/ru-ru/dotnet/desktop/wpf/)

[![GitHub stars](https://img.shields.io/github/stars/Yvunglord/CKL_Studio?style=social)](https://github.com/Yvunglord/CKL_Studio/stargazers)
[![Last Commit](https://img.shields.io/github/last-commit/Yvunglord/CKL_Studio)](https://github.com/Yvunglord/CKL_Studio/commits/main)
[![Roslyn Analyzers](https://img.shields.io/badge/Roslyn%20Analyzers-0%20warnings-green?logo=.net)](https://github.com/Yvunglord/CKL_Studio/actions)

</div>

## 📝 Описание проекта
CKL Studio — это прототип инструментальной системы для работы с **алгеброй динамических отношений** (циклограммами). Проект предназначен для:
- Моделирования временных зависимостей во множестве сфер, таких как производстве, логистике и управлении проектами
- Визуализации сложных динамических систем
- Анализа и оптимизации процессов

## 🌟 Основные функции
| Функция                     | Описание                                                                 |
|-----------------------------|--------------------------------------------------------------------------|
| **Редактор отношений**      | Создание и модификация динамических отношений через интуитивный интерфейс |
| **Визуализация**            | Автоматическое построение диаграмм циклограмм                           |
| **Операции над отношениями**| Поддержка различных типов операций (алгебраические, теоретико-множественные и т.д.       |
| **Гибкий интерфейс**        | Режимы работы: file, edit, unary, binary, semantic, window, view                        |
| **История действий**        | Полная поддержка Undo/Redo системы                                      |

## 🧮 Поддерживаемые операции
### Бинарные операции
| Операция               | Обозначение | Команда              | Пример использования |
|------------------------|-------------|----------------------|----------------------|
| Объединение            | ∪           | `UnionCommand`       | A ∪ B                |
| Пересечение            | ∩           | `IntersectionCommand`| A ∩ B                |
| Разность               | \           | `DifferenceCommand`  | A \ B                |
| Композиция             | ∘           | `CompositionCommand` | A ∘ B                |
| Семантическое объединение | -       | `SemanticUnionCommand` | A ⊔ B              |

### Унарные операции
| Операция               | Обозначение | Команда              |
|------------------------|-------------|----------------------|
| Инверсия               | ¬           | `InversionCommand`   |
| Транспозиция           | ¯           | `TranspositionCommand` |

### Временные операции
| Операция               | Команда              |
|------------------------|----------------------|
| Изменить глобальный интервал				| `TimeTransformCommand`   |
| Левое предшествование				| `LeftPrecedenceCommand` |
| Правое предшествование				| `RightPrecedenceCommand` |
| Левое следование				| `LeftContinuationCommand` |
| Правое следование				| `RightContinuationCommand` 

## 🛠 Технологии
- **Ядро**: C# 12, .NET 8.0
- **Интерфейс**: WPF, XAML, MVVM
- **Дополнительно**:
  - `Dependency Injection` для гибкости архитектуры
  - `CKLLib и CKLDrawing` для работы с динамическими отношениями
  - GitHub Actions для CI 

## 🚀 Установка и запуск
### Требования
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- IDE (рекомендуется):
  - Visual Studio 2022
  - Rider

### Запуск из исходников
```bash
git clone https://github.com/Yvunglord/CKL_Studio.git
cd CKL_Studio
dotnet build
dotnet run --project CKL.Studio