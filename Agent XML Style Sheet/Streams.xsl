<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
xmlns:xs="http://www.w3.org/2001/XMLSchema" 
xmlns:fn="http://www.w3.org/2005/xpath-functions" 
xmlns:m="urn:mtconnect.org:MTConnectStreams:1.3"
>

	<xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>
		
	<xsl:template match="/">
	
			<head>
			
				<title>MTConnect Device Streams</title>
				
				<link rel="stylesheet"
				href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"
				integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" 
				crossorigin="anonymous"/>
				
				<script src="/styles/GetSample.js"></script>
				
				<!-- Latest compiled and minified JavaScript -->
				<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" 
				integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"
				crossorigin="anonymous"></script>
														
			</head>
			
			<body>
			
				<nav class="navbar navbar-default navbar-fixed-top">
				  <div class="container-fluid">
					<!-- Brand and toggle get grouped for better mobile display -->
					<div class="navbar-header">
					
					  <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
						<span class="sr-only">Toggle navigation</span>
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
					  </button>
					  
					  <a class="navbar-brand" style="padding: 5px 20px;" href="http://www.mtconnect.org">
						<img alt="Brand" src="/styles/MTConnect-Logo.png" height="40"/>
					  </a>
					  
					</div>

					<!-- Collect the nav links, forms, and other content for toggling -->
					<div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
					  <ul class="nav navbar-nav">
					  
						<li><a href="../probe">Probe</a></li>
						
						<li><a href="../current">Current</a></li>
						
						<li><a href="../sample">Sample</a></li>
						
					  </ul>
					  
					  <div class="navbar-form navbar-left">
					  
						<div class="form-group">
							<input id="fromText" type="text" class="form-control" style="margin-right: 10px;" placeholder="From"/>
							<input id="countText" type="text" class="form-control" style="margin-right: 10px;" placeholder="Count"/>
						</div>

						<button onclick="getSample();" class="btn btn-default">Get Sample</button>
					  
					  </div>
					  					  
					</div><!-- /.navbar-collapse -->
				  </div><!-- /.container-fluid -->
				</nav>
				
				<div class="container-fluid" style="margin-top: 60px;">
					
					<p>
						<xsl:apply-templates select="/m:MTConnectStreams/m:Header" />
					</p>
					
					<hr/>
					
					<xsl:apply-templates select="/m:MTConnectStreams/m:Streams/m:DeviceStream" />
				
				</div>
				
				<footer class="footer" style="margin-top: 20px; margin-bottom: 20px;">
				
				  <div class="container">
				  
					<div class="row">
					
						<p class="col-md-3 text-muted">This template is designed by Feenux LLC and is available for use through the MIT license.</p>
						
						<p class="col-md-3 text-muted">Source code for this template is available at 
							<a href="https://github.com/TrakHound/MTConnect-Agent-Stylesheet">GitHub</a>
						</p>
					
						<p class="col-md-3 text-muted">© 2016 Feenux LLC, All Rights Reserved</p>

					</div>

				  </div>
				  
				</footer>
											
			</body>
			
	</xsl:template>
	
	<xsl:template match="m:DeviceStream">
	
		<div class="panel panel-default">
		
			<div class="panel-heading">
			
				<div class="container-fluid">
				
					<div class="row">
					
						<div class="col-sm-2">
						
							<h6 style="margin-bottom: 0px;">Device</h6>
						
							<h2 style="margin-top: 0px; margin-bottom: 5px;"><xsl:value-of select="@name"/></h2>
						
						</div>
						
						<div class="col-sm-2">
						
							<h6 style="margin-bottom: 0px;">UUID</h6>

							<h4 style="margin-top: 0px; margin-bottom: 5px;"><xsl:value-of select="@uuid"/></h4>
						
						</div>
					
					</div>
				
				</div>
						
			</div>
			
			<div class="panel-body">
			
				<xsl:apply-templates select="m:ComponentStream"/>
			
			</div>
				
		</div>
			
	</xsl:template>
	
	<xsl:template match="m:Header">
	
		<div class="panel panel-default">
			<div class="panel-heading">
				<i class="fa fa-bar-chart-o fa-fw"></i>Agent Information
			</div>
			
			<div class="panel-body">
				
				<ul class="list-group">
				
					<xsl:for-each select="@*">
					
						<li class="list-group-item col-md-3">
							
							<h6 class="list-group-item-header">

								<xsl:value-of select="name()"/>
								
							</h6>

							<h4 class="list-group-item-text">
							
								<xsl:value-of select="." />
								
							</h4>
												
						</li>
						
					</xsl:for-each>
					
				</ul>
			
			</div>
			
		</div>
	
	</xsl:template>
	
	<xsl:template match="m:ComponentStream">
	
		<div style="margin-left: 10px;">
		
			<h6 style="margin-bottom: 0px;"><xsl:value-of select="@component" /></h6>
							
			<h3 style="margin-top: 0px; margin-bottom: 5px;"><xsl:value-of select="@name" /></h3>
			
		</div>
	
		<div class="panel-group">
				
			<xsl:apply-templates select="	m:Samples"/>
			<xsl:apply-templates select="	m:Events"/>
			<xsl:apply-templates select="	m:Condition"/>
		
		</div>
				
	</xsl:template>
	
	<xsl:template match="*">
	
		<div class="panel panel-default">
			<div class="panel-heading">
				<i class="fa fa-bar-chart-o fa-fw"></i>
				<xsl:value-of select="name()"/>
			</div>
			
			<div class="panel-body">	

				<div class="table-responsive">

					<table class="table table-hover">
						<thead>
							<th>Timestamp</th><th>Type</th><th>Sub Type</th><th>Name</th><th>Id</th>
							<th>Sequence</th><th>Value</th>
						</thead>
						<tbody>
							<xsl:for-each select="*">
								<tr>
									<td><xsl:value-of select="@timestamp"/></td>
									<td><xsl:value-of select="name()"/></td>
									<td><xsl:value-of select="@subType"/></td>
									<td><xsl:value-of select="@name"/></td>
									<td><xsl:value-of select="@dataItemId"/></td>
									<td><xsl:value-of select="@sequence"/></td>
									<td><xsl:value-of select="."/></td>
								</tr>
							</xsl:for-each>
						</tbody>
					</table>

				</div>			
	
			</div>
			
		</div>	

	</xsl:template>
	
</xsl:stylesheet>
