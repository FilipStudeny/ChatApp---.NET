/* eslint-disable react/jsx-no-undef */
/* eslint-disable react/button-has-type */
import { CssBaseline, Grid } from "@mui/material";
import React, { useState } from "react";

import Header from "../components/Header";

const Layout: React.FunctionComponent = () => {
	const [leftSidebarOpen, setLeftSidebarOpen] = useState(true);
	const [rightSidebarOpen, setRightSidebarOpen] = useState(true);

	const toggleLeftSidebar = () => {
		setLeftSidebarOpen(!leftSidebarOpen);
	};

	const toggleRightSidebar = () => {
		setRightSidebarOpen(!rightSidebarOpen);
	};

	return (
		<>
			<CssBaseline />
			<Grid container spacing={0}>
				<Grid item>asda </Grid>
				<Grid item xs='auto'>
					<Header />
				</Grid>
				<Grid item>asdsa </Grid>
			</Grid>
		</>
	);
};

export default Layout;

/*
<Box sx={{ display: "flex" }}>
			
			<Sidebar open={rightSidebarOpen} position='right' />
			<Box
				component='main'
				sx={{
					flexGrow: 1,
					p: 3,
					marginLeft: leftSidebarOpen ? "240px" : 0,
					marginRight: rightSidebarOpen ? "240px" : 0,
				}}
			>
				<Feed />
			</Box>
		</Box>
*/
