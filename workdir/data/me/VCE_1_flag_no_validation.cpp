import semmle.code.java.dataflow.FlowSources

class SpringServletInputParameterSource extends RemoteFlowSource {
    SpringServletInputParameterSource() {
      this.asParameter() = any(SpringRequestMappingParameter srmp | 
                                                    srmp.isTaintedInput())
    }

    override string getSourceType() {
        result = "Spring servlet input parameter" }
  }

from SpringServletInputParameterSource c
where not c.asParameter()
                   .getAnAnnotation()
                   .getType()
                   .hasQualifiedName("javax.validation", "Valid")
select c