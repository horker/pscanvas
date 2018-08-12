#requires -PSEdition Desktop

Set-StrictMode -Version 4

[Horker.Canvas.WpfWindow]::OpenRootWindow()

############################################################
# Extension methods
############################################################

#$METHOD_LIST = @(
#  [PSCustomObject]@{
#    TargetClass = "CNTK.Function"
#    ClassInfo = [Horker.PSCNTK.FunctionMethods]
#    MethodNames = @(
#      "Find"
#      "Invoke"
#      "AsTree"
#    )
#  }
#
#  [PSCustomObject]@{
#    TargetClass = "CNTK.Value"
#    ClassInfo = [Horker.PSCNTK.ValueMethods]
#    MethodNames = @(
#      "AsString"
#      "ToDataSource"
#      "ToArray"
#    )
#  }
#)
#
#foreach ($l in $METHOD_LIST) {
#  $ci = $l.ClassInfo
#  $target = $l.TargetClass
#  foreach ($m in $l.MethodNames) {
#    $mi = $ci.GetMethod($m)
#    Update-TypeData -TypeName $target -MemberName $m -MemberType CodeMethod -Value $mi -Force
#  }
#}
