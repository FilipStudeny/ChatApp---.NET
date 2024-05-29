import SearchIcon from "@mui/icons-material/Search";
import { AppBar, Button, Divider, Grid, InputBase, Link as MuiLink, Stack, styled } from "@mui/material";
import React from "react";
import { Link } from "react-router-dom";

const Search = styled("div")(({ theme }) => ({
	borderRadius: theme.shape.borderRadius,
	backgroundColor: theme.palette.common.white,
	width: "100%",
	[theme.breakpoints.up("sm")]: {
		marginLeft: theme.spacing(1),
		width: "auto",
    },
}));

const SearchIconWrapper = styled("div")(({ theme }) => ({
	padding: theme.spacing(1, 1),
	position: "absolute",
	pointerEvents: "none",
	display: "flex",
	alignItems: "center",
	justifyContent: "center",
    color: "gray",
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
	width: "100%",
	"& .MuiInputBase-input": {
		padding: theme.spacing(1, 1, 1, 0),
		paddingLeft: `calc(1em + ${theme.spacing(4)})`,
		transition: theme.transitions.create("width"),
		[theme.breakpoints.up("sm")]: {
			width: "12ch",
			"&:focus": {
				width: "20ch",
			},
		},
	},
}));

const Header: React.FunctionComponent = () => (
	<AppBar position='fixed'>
		<Grid container spacing={0}>
			<Grid item xs>
				<MuiLink underline='none' component={Link} to='/' color='inherit'>
					<Button sx={{ m: 1 }} variant='outlined' color='inherit'>
						Home
					</Button>
				</MuiLink>
			</Grid>
			<Grid item xs={6}>
				<Search>
					<SearchIconWrapper>
						<SearchIcon />
					</SearchIconWrapper>
					<StyledInputBase placeholder='Search…' inputProps={{ "aria-label": "search" }} />
				</Search>
			</Grid>
			<Grid item xs>
				<Stack direction='row' justifyContent='flex-end'>
					<MuiLink underline='none' component={Link} to='/login' color='inherit'>
						<Button sx={{ m: 1 }} variant='outlined' color='inherit'>
							Sign in
						</Button>
					</MuiLink>
					<MuiLink underline='none' component={Link} to='/register' color='inherit'>
						<Button sx={{ m: 1 }} variant='outlined' color='inherit'>
							Sign up
						</Button>
					</MuiLink>
				</Stack>
			</Grid>
		</Grid>
	</AppBar>
);

export default Header;

/*
		<Stack>
			<MuiLink underline='none' component={Link} to='/' color='inherit'>
				<Button sx={{ m: 1 }} variant='outlined' color='inherit'>
					Home
				</Button>
			</MuiLink>
				<MuiLink underline='none' component={Link} to='/login' color='inherit'>
					<Button sx={{ m: 1 }} variant='outlined' color='inherit'>
						Sign in
					</Button>
				</MuiLink>
				<MuiLink underline='none' component={Link} to='/register' color='inherit'>
					<Button sx={{ m: 1 }} variant='outlined' color='inherit'>
						Sign up
					</Button>
				</MuiLink>
		</Stack>

*/
