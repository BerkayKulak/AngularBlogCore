﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularBlogCore.API.Models;
using AngularBlogCore.API.Responses;

namespace AngularBlogCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly UdemyAngularBlogDBContext _context;

        public ArticlesController(UdemyAngularBlogDBContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = _context.Articles.Include(a => a.Category).Include(b => b.Comments).OrderByDescending(x => x.PublishDate).ToList().Select(y => new ArticleResponse()
            {
                Id = y.Id,
                Title = y.Title,
                Picture = y.Picture,
                Category = new CategoryResponse() { Id = y.Category.Id, Name = y.Category.Name },
                CommentCount = y.Comments.Count,

                ViewCount = y.ViewCount,
                PublishDate = y.PublishDate
            });
            return Ok(articles);
        }


     

        [HttpGet("{page}/{pageSize}")]
        public IActionResult GetArticle(int page = 1, int pageSize = 5)
        {
            System.Threading.Thread.Sleep(1500);
            try
            {
                IQueryable<Article> query;

                query = _context.Articles.Include(x => x.Category).
                    Include(y=>y.Comments).OrderByDescending(z=>z.PublishDate);

                int totalCount = query.Count();

                var articlesResponse = query.Skip(((pageSize * (page - 1)))).
                    Take(5).ToList().Select(x=>new ArticleResponse()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        ContentMain = x.ContentMain,
                        ContentSummary = x.ContentSummary,
                        Picture = x.Picture,
                        ViewCount = x.ViewCount,
                        CommentCount = x.Comments.Count,
                        Category =new CategoryResponse(){Id = x.Category.Id,Name = x.Category.Name}
                    
                    
                    });

                var result = new
                {
                    TotalCount = totalCount,
                    Articles = articlesResponse
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            
        }


        //localhost/api/articles/GetArticlesWithCategory/2/1/5
        [HttpGet]
        [Route("GetArticlesWithCategory/{categoryId}/{page}/{pageSize}")]
        public IActionResult GetArticlesWithCategory(int categoryId, int page = 1, int pageSize = 5)
        {
            System.Threading.Thread.Sleep(1500);
            IQueryable<Article> query = _context.Articles.Include(x => x.Category).Include(y => y.Comments).Where(z => z.CategoryId == categoryId).OrderByDescending(x => x.PublishDate);

            var queryResult = ArticlePagination(query, page, pageSize);

            var result = new
            {
                TotalCount = queryResult.Item2,
                Articles = queryResult.Item1
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("SearchArticles/{searchText}/{page}/{pageSize}")]
        public IActionResult SearchArticles(string searchText, int page = 1, int pageSize = 5)
        {
            IQueryable<Article> query;

            query = _context.Articles.Include(x => x.Category).Include(y => y.Comments).Where(z => z.Title.Contains(searchText)).OrderByDescending(f => f.PublishDate);

            var resultQuery = ArticlePagination(query, page, pageSize);

            var result = new
            {
                Articles = resultQuery.Item1,
                TotalCount = resultQuery.Item2
            };
            
            return Ok(result);
        }

        [HttpGet]
        [Route("GetArticlesByMostView")]
        public IActionResult GetArticlesByMostView()
        {
            System.Threading.Thread.Sleep(1500);
            var articles = _context.Articles.OrderByDescending(x => x.ViewCount).Take(5).Select(x => new ArticleResponse()
            {
                Title = x.Title,
                Id = x.Id
            });

            return Ok(articles);
        }


        [HttpGet]
        [Route("GetArticlesArchive")]
        public IActionResult GetArticlesArchive()
        {
            System.Threading.Thread.Sleep(1000);
            var info = CultureInfo.CreateSpecificCulture("tr");
            var query = _context.Articles.GroupBy(x => new { x.PublishDate.Year, x.PublishDate.Month }).Select(y =>
                new
                {
                    year = y.Key.Year,
                    month = y.Key.Month,
                    count = y.Count(),
                    monthName = new DateTime(y.Key.Year, y.Key.Month, 1).ToString("MMMM", info)
                });

            return Ok(query);
        }

        [HttpGet]
        [Route("GetArticleArchiveList/{year}/{month}/{page}/{pageSize}")]
        public IActionResult GetArticleArchiveList(int year, int month, int page, int pageSize)
        {
            System.Threading.Thread.Sleep(1700);

            IQueryable<Article> query;
            query = _context.Articles.Include(x => x.Category).
                Include(y => y.Comments).
                Where(z => z.PublishDate.Year == year && z.PublishDate.Month == month).
                OrderByDescending(f => f.PublishDate);

            var resultQuery = ArticlePagination(query, page, pageSize);

            var result = new
            {
                Articles = resultQuery.Item1,
                TotalCount = resultQuery.Item2
            };

            return Ok(result);
        }


        [Route("ArticleViewCountUp/{id}")]
        [HttpGet()]
        public IActionResult ArticleViewCountUp(int id)
        {
            Article article = _context.Articles.Find(id);
            article.ViewCount += 1;
            _context.SaveChanges();
            return Ok();
        }



        // GET: api/Articles/5
        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            System.Threading.Thread.Sleep(1500);

            var article = _context.Articles.Include(x => x.Category).
                Include(y => y.Comments)
                .FirstOrDefault(z => z.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            ArticleResponse articleResponse = new ArticleResponse()
            {
                Id = article.Id,
                Title = article.Title,
                ContentMain = article.ContentMain,
                ContentSummary = article.ContentSummary,
                Picture = article.Picture,
                PublishDate = article.PublishDate,
                ViewCount = article.ViewCount,
                Category = new CategoryResponse()
                {
                    Id = article.Category.Id,
                    Name = article.Category.Name
                },
                CommentCount = article.Comments.Count
            };

            return Ok(articleResponse);

        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        private System.Tuple<IEnumerable<ArticleResponse>, int> ArticlePagination(IQueryable<Article> query, int page,
            int pageSize)
        {
            int totalCount = query.Count();

            var articlesResponse = query.Skip(((pageSize * (page - 1)))).
                Take(pageSize).ToList().Select(x => new ArticleResponse()
                {
                    Id = x.Id,
                    Title = x.Title,
                    ContentMain = x.ContentMain,
                    ContentSummary = x.ContentSummary,
                    Picture = x.Picture,
                    ViewCount = x.ViewCount,
                    CommentCount = x.Comments.Count,
                    Category = new CategoryResponse() { Id = x.Category.Id, Name = x.Category.Name }


                });

            return new System.Tuple<IEnumerable<ArticleResponse>, int>(articlesResponse, totalCount);

        }
    }
}
