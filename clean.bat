@echo off

rmdir /s /q .vs

rmdir /s /q build
rmdir /s /q build_installer

rmdir /s /q LenovoYogaToolkit.Lib\bin
rmdir /s /q LenovoYogaToolkit.Lib\obj

rmdir /s /q LenovoYogaToolkit.Lib.Automation\bin
rmdir /s /q LenovoYogaToolkit.Lib.Automation\obj

rmdir /s /q LenovoYogaToolkit.WPF\bin
rmdir /s /q LenovoYogaToolkit.WPF\obj

rmdir /s /q LenovoYogaToolkit.SpectrumTester\bin
rmdir /s /q LenovoYogaToolkit.SpectrumTester\obj
