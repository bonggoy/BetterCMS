﻿using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.ImagesGallery.Models;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.SaveAlbum
{
    public class SaveAlbumCommand : CommandBase, ICommand<AlbumEditViewModel, AlbumEditViewModel>
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAlbumCommand" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public SaveAlbumCommand(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public AlbumEditViewModel Execute(AlbumEditViewModel request)
        {
            unitOfWork.BeginTransaction();

            var isNew = request.Id.HasDefaultValue();
            Album album;

            if (isNew)
            {
                album = new Album();
            }
            else
            {
                album = repository.AsQueryable<Album>(w => w.Id == request.Id).FirstOne();
            }

            album.Title = request.Title;
            album.Version = request.Version;
            album.CoverImage =
                request.CoverImage != null && request.CoverImage.ImageId.HasValue
                    ? Repository.AsProxy<MediaImage>(request.CoverImage.ImageId.Value)
                    : null;
            album.Folder =
                request.Folder != null && request.Folder.FolderId.HasValue
                    ? Repository.AsProxy<MediaFolder>(request.Folder.FolderId.Value)
                    : null;

            repository.Save(album);
            unitOfWork.Commit();

            if (isNew)
            {
                Events.ImageGalleryEvents.Instance.OnAlbumCreated(album);
            }
            else
            {
                Events.ImageGalleryEvents.Instance.OnAlbumUpdated(album);
            }

            return new AlbumEditViewModel
                {
                    Id = album.Id,
                    Version = album.Version,
                    Title = album.Title
                };
        }
    }
}