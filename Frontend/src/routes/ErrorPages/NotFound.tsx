import { Button, Container, Link, Typography } from "@mui/material";
import React from "react";
import { Link as RouterLink } from "react-router-dom";

const NotFound: React.FunctionComponent = () => (
	<Container sx={{ m: "auto", mt: 5 }}>
		<Typography align='center' variant='h3' component='h2' color='white'>
			Page not found
		</Typography>
		<Typography align='center' variant='h5' component='p'>
			<Link component={RouterLink} to='/'>
				<Button sx={{ m: 1 }} variant='contained'>
					Go back
				</Button>
			</Link>
		</Typography>
	</Container>
);

export default NotFound;
